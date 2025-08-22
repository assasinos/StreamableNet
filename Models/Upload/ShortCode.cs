using System.Text.Json.Serialization;

namespace StreamableNet.Models.Upload
{
    public class ShortCode
    {
        [JsonPropertyName("credentials")]
        public CredentialsFields Credentials { get; set; } = default!;

        [JsonPropertyName("fields")]
        public UploadFields Fields { get; set; } = default!;

        [JsonPropertyName("transcoder_options")]
        public TranscoderOptionsFields TranscoderOptions { get; set; } = default!;

        [JsonPropertyName("shortcode")]
        public string Shortcode { get; set; } = string.Empty;


        public class TranscoderOptionsFields
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("token")]
            public string Token { get; set; }

            [JsonPropertyName("shortcode")]
            public string Shortcode { get; set; }

            [JsonPropertyName("size")]
            public long Size { get; set; }
        }

        public class CredentialsFields
        {
            [JsonPropertyName("accessKeyId")]
            public string AccessKeyId { get; set; } = string.Empty;

            [JsonPropertyName("secretAccessKey")]
            public string SecretAccessKey { get; set; } = string.Empty;

            [JsonPropertyName("sessionToken")]
            public string SessionToken { get; set; } = string.Empty;
        }

        public class UploadFields
        {
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;

            [JsonPropertyName("acl")]
            public string Acl { get; set; } = string.Empty;

            [JsonPropertyName("bucket")]
            public string Bucket { get; set; } = string.Empty;

            [JsonPropertyName("X-Amz-Algorithm")]
            public string XAmzAlgorithm { get; set; } = string.Empty;

            [JsonPropertyName("X-Amz-Credential")]
            public string XAmzCredential { get; set; } = string.Empty;

            [JsonPropertyName("X-Amz-Date")]
            public string XAmzDate { get; set; } = string.Empty;

            [JsonPropertyName("X-Amz-Security-Token")]
            public string XAmzSecurityToken { get; set; } = string.Empty;

            [JsonPropertyName("Policy")]
            public string Policy { get; set; } = string.Empty;

            [JsonPropertyName("X-Amz-Signature")]
            public string XAmzSignature { get; set; } = string.Empty;
        }

    }
}
