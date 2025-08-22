using StreamableNet.Consts;
using StreamableNet.Models.Video;
using System.Net.Http.Json;

namespace StreamableNet.Clients
{
    /// <summary>
    /// Provides video operations for the Streamable API.
    /// </summary>
    public class VideoClient
    {
        private readonly StreamableOptions _streamableOptions;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoClient"/> class.
        /// </summary>
        /// <param name="client">HTTP client for API requests.</param>
        /// <param name="streamableOptions">Streamable API configuration options.</param>
        internal VideoClient(HttpClient client, StreamableOptions streamableOptions)
        {
            _streamableOptions = streamableOptions ?? throw new ArgumentNullException(nameof(streamableOptions));
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Retrieves the authenticated user's video collection.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection containing the user's videos and total count or null if nothing returned.</returns>
        public async Task<VideoCollection?> GetVideosAsync(CancellationToken cancellationToken = default)
        {

            var response = await _httpClient.GetFromJsonAsync<VideoCollection>(_streamableOptions.VideoEndpoint, cancellationToken).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Retrieves a specific video by its ID.
        /// </summary>
        /// <param name="videoId">The unique identifier of the video.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The video details, or null if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when videoId is null or whitespace.</exception>
        public async Task<Video?> GetVideoAsync(string videoId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(videoId))
            {
                throw new ArgumentException("Video ID cannot be null or whitespace", nameof(videoId));
            }
            var response = await _httpClient.GetFromJsonAsync<Video>($"{_streamableOptions.VideoEndpoint}/{videoId}", cancellationToken).ConfigureAwait(false);

            return response;
        }
    }

}
