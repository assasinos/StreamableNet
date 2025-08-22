using StreamableNet.Consts;
using StreamableNet.Models.Auth;

namespace StreamableNet.Auth
{
    /// <summary>
    /// Defines the contract for authentication providers
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Authenticates with the Streamable API.
        /// </summary>
        /// <param name="client">HTTP client for the request.</param>
        /// <param name="streamableOptions">API configuration options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The authenticated user's account data.</returns>
        Task<AuthenticatedUser> AuthenticateAsync(HttpClient client, StreamableOptions streamableOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// Logs out the current user session.
        /// </summary>
        /// <param name="client">HTTP client for the request.</param>
        /// <returns>Task representing the logout operation.</returns>
        Task LogoutAsync(HttpClient client);
    }
}
