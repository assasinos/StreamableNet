using StreamableNet.Consts;
using StreamableNet.Models.Upload;
using StreamableNet.Utils;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static StreamableNet.Models.Upload.ShortCode;

namespace StreamableNet.Clients
{
    public class UploadClient
    {

        private const int DefaultBufferSize = 81920; // 80 KB

        private readonly StreamableOptions _streamableOptions;
        private readonly HttpClient _httpClient;

        internal UploadClient(HttpClient client, StreamableOptions streamableOptions)
        {
            _streamableOptions = streamableOptions ?? throw new ArgumentNullException(nameof(streamableOptions));
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> UploadFileAsync(string filePath, string? title, CancellationToken cancellationToken = default)
        {
            // Validate file path
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                throw new ArgumentException("File path is invalid or file does not exist.", nameof(filePath));
            }
            // Get file size
            var fileInfo = new System.IO.FileInfo(filePath);
            long fileSize = fileInfo.Length;
            if (fileSize <= 0)
            {
                throw new ArgumentException("File size must be greater than zero.", nameof(filePath));
            }
            // Get short code
            var shortCode = await GetShortCodeAsync(fileSize, cancellationToken).ConfigureAwait(false);
            if (shortCode == null || shortCode.Fields == null || shortCode.Credentials == null)
            {
                throw new InvalidOperationException("Short code or fields are not properly initialized.");
            }


            await NotifyUploadInitializeAsync(fileInfo.Name, fileSize, title, shortCode.Shortcode, cancellationToken).ConfigureAwait(false);

            await PutFileToStorageAsync(fileInfo.FullName, shortCode, cancellationToken).ConfigureAwait(false);

            await NotifyUploadCompleteAsync(shortCode.Shortcode, cancellationToken).ConfigureAwait(false);

            await TranscodeUploadAsync(shortCode.TranscoderOptions, cancellationToken).ConfigureAwait(false);

            return shortCode.Shortcode;

        }

        private async Task NotifyUploadInitializeAsync(string name, long fileSize, string? title, string shortcode, CancellationToken cancellationToken)
        {
            var requestUri = this._streamableOptions.InitializeUploadEndpoint(shortcode);

            var titleToUse = string.IsNullOrWhiteSpace(title) ? name : title;

            var json = new
            {
                original_size = fileSize,
                original_name = name,
                upload_source = "web",
                title = titleToUse
            };

            using var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(json), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HttpRequestException($"Failed to notify upload initialization. Status code: {response.StatusCode}, Error: {errorMessage}");
            }
        }

        private async Task PutFileToStorageAsync(string filePath, ShortCode shortCode, CancellationToken cancellationToken)
        {

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, shortCode.TranscoderOptions.Url);

            //Setup S3 upload headers
            httpRequestMessage.Headers.Add("x-amz-acl", shortCode.Fields.Acl);
            httpRequestMessage.Headers.Add("x-amz-content-sha256", "UNSIGNED-PAYLOAD");
            httpRequestMessage.Headers.Add("x-amz-date", shortCode.Fields.XAmzDate);
            httpRequestMessage.Headers.Add("x-amz-security-token", shortCode.Fields.XAmzSecurityToken);

            httpRequestMessage.Headers.Add("x-amz-user-agent", "aws-sdk-js/2.1530.0 callback");

            var canonicalRequest = BuildCanonicalRequest(shortCode);

            //Get hash of the canonical request
            var canonicalRequestHash = AwsSigV4Utils.ComputeSha256(canonicalRequest);
            var amzCredentialSplit = shortCode.Fields.XAmzCredential.Split('/');
            var date = amzCredentialSplit[1];
            var region = amzCredentialSplit[2];
            var service = amzCredentialSplit[3];
            var credentialScope = $"{date}/{region}/{service}/aws4_request";

            var stringToSign = $"AWS4-HMAC-SHA256\n{shortCode.Fields.XAmzDate}\n{credentialScope}\n{canonicalRequestHash}";

            var signature = AwsSigV4Utils.CalculateSignature(
                shortCode.Credentials.SecretAccessKey,
                date,
                region,
                service,
                stringToSign
            );


            //Setup AuthHeader
            var authHeader = $"{shortCode.Fields.XAmzAlgorithm} Credential={shortCode.Fields.XAmzCredential}, " +
                "SignedHeaders=host;x-amz-acl;x-amz-content-sha256;x-amz-date;x-amz-security-token;x-amz-user-agent, " +
                $"Signature={signature}";
            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", authHeader);

            await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, useAsync: true);
            using var content = new StreamContent(fileStream);
            httpRequestMessage.Content = content;
            //Set content type to octet stream
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            using var response = await _httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HttpRequestException($"Failed to upload file. Status code: {response.StatusCode}, Error: {errorMessage}");
            }

        }

        //Send event that upload is complete
        public async Task NotifyUploadCompleteAsync(string shortcode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(shortcode))
            {
                throw new ArgumentException("Shortcode cannot be null or empty.", nameof(shortcode));
            }

            var requestUri = _streamableOptions.NotifyUploadCompleteEndpoint(shortcode);
            var jsonContent = new StringContent("{\"event\":\"complete\"}", Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(requestUri, jsonContent, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HttpRequestException($"Failed to notify upload completion. Status code: {response.StatusCode}, Error: {errorMessage}");
            }
        }

        public async Task TranscodeUploadAsync(TranscoderOptionsFields transcoderOptions, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(transcoderOptions.Shortcode))
            {
                throw new ArgumentException("Shortcode cannot be null or empty.", nameof(transcoderOptions.Shortcode));
            }
            var requestUri = _streamableOptions.TranscodeUploadEndpoint(transcoderOptions.Shortcode);

            var json = new
            {
                upload_source = "web",
                url = transcoderOptions.Url,
                token = transcoderOptions.Token,
                shortcode = transcoderOptions.Shortcode,
                size = transcoderOptions.Size
            };

            using var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(json), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new HttpRequestException($"Failed to transcode upload. Status code: {response.StatusCode}, Error: {errorMessage}");
            }
        }

        private static string BuildCanonicalRequest(ShortCode shortCode)
        {
            var uri = new Uri(shortCode.TranscoderOptions.Url);
            var path = uri.AbsolutePath;

            const string signedHeaders = "host;x-amz-acl;x-amz-content-sha256;x-amz-date;x-amz-security-token";

            var sb = new StringBuilder(256);
            sb.Append("PUT\n");
            sb.Append(path).Append('\n');              // canonical URI and empty canonical query string follows
            sb.Append('\n');                           // empty canonical query string (no query)
            sb.Append("host:").Append(uri.Host).Append('\n');
            sb.Append("x-amz-acl:").Append(shortCode.Fields.Acl).Append('\n');
            sb.Append("x-amz-content-sha256:UNSIGNED-PAYLOAD").Append('\n');
            sb.Append("x-amz-date:").Append(shortCode.Fields.XAmzDate).Append('\n');
            sb.Append("x-amz-security-token:").Append(shortCode.Fields.XAmzSecurityToken).Append('\n');
            sb.Append('\n');                           // empty line between headers and signed headers list
            sb.Append(signedHeaders).Append('\n');
            sb.Append("UNSIGNED-PAYLOAD");

            return sb.ToString();
        }


        private async Task<ShortCode> GetShortCodeAsync(long fileSize, CancellationToken cancellationToken = default)
        {
            var requestUri = $"{_streamableOptions.ShortCodeEndpoint}?size={fileSize}&version=unknown";
            var response = await _httpClient.GetFromJsonAsync<ShortCode>(requestUri, cancellationToken).ConfigureAwait(false);
            return response ?? throw new InvalidOperationException("Failed to retrieve short code.");
        }



    }
}
