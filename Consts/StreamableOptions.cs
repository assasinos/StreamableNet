using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
