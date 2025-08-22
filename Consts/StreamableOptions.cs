namespace StreamableNet.Consts
{

    public class StreamableOptions
    {
        public string BaseApiUrl { get; init; } = "https://api-f.streamable.com";
        public string AuthBaseUrl { get; init; } = "https://ajax.streamable.com";

        // Relative endpoints
        public string LogInEndpointPath { get; init; } = "/check";
        public string VideoEndpointPath { get; init; } = "/api/v1/videos";
        public string ShortCodeEndpointPath { get; init; } = "/api/v1/uploads/shortcode";

        // Full endpoint properties for convenience and backward compatibility
        public string LogInEndpoint => $"{AuthBaseUrl}{LogInEndpointPath}";
        public string VideoEndpoint => $"{BaseApiUrl}{VideoEndpointPath}";
        public string ShortCodeEndpoint => $"{BaseApiUrl}{ShortCodeEndpointPath}";

        public string NotifyUploadCompleteEndpoint(string shortcode) => $"{BaseApiUrl}/api/v1/uploads/{shortcode}/track";
        public string TranscodeUploadEndpoint(string shortcode) => $"{BaseApiUrl}/api/v1/transcode/{shortcode}";
        public string InitializeUploadEndpoint(string shortcode) => $"{BaseApiUrl}/api/v1/videos/{shortcode}/initialize";
    }

}
