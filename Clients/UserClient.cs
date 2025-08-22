using StreamableNet.Auth;
using StreamableNet.Consts;
using StreamableNet.Models.Auth;

namespace StreamableNet.Clients
{
    /// <summary>
    /// Provides user account operations for the Streamable API.
    /// </summary>
    public class UserClient
    {
        private readonly IAuthProvider _authProvider;
        private readonly StreamableOptions _streamableOptions;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for API requests.</param>
        /// <param name="authProvider">The authentication provider.</param>
        /// <param name="streamableOptions">Streamable API configuration options.</param>
        internal UserClient(HttpClient httpClient, IAuthProvider authProvider, StreamableOptions streamableOptions)
        {
            _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            _streamableOptions = streamableOptions ?? throw new ArgumentNullException(nameof(streamableOptions));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Authenticates the user and returns their account information.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The authenticated user's account data.</returns>
        /// <exception cref="StreamableAuthException">Thrown when authentication fails.</exception>
        public async Task<AuthenticatedUser> AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            return await _authProvider.AuthenticateAsync(_httpClient, _streamableOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
