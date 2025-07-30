using StreamableNet.Consts;
using StreamableNet.Exceptions.Auth;
using StreamableNet.Exceptions.Base;
using StreamableNet.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StreamableNet.Auth
{
    /// <summary>
    /// Provides username/password authentication for the Streamable API.
    /// </summary>
    public class BasicAuthProvider : IAuthProvider
    {
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance with the specified credentials.
        /// </summary>
        /// <param name="username">Streamable username.</param>
        /// <param name="password">Streamable password.</param>
        public BasicAuthProvider(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            }
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Authenticates with the Streamable API using username and password.
        /// </summary>
        /// <param name="client">HTTP client for the request.</param>
        /// <param name="streamableOptions">API configuration options.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The authenticated user's account data.</returns>
        public async Task<AuthenticatedUser> AuthenticateAsync(HttpClient client, StreamableOptions streamableOptions, CancellationToken cancellationToken = default)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            try
            {
                var credentials = new { username = _username, password = _password };
                using var content = new StringContent(
                    JsonSerializer.Serialize(credentials),
                    Encoding.UTF8,
                    "application/json");

                using var response = await client.PostAsync(streamableOptions.LogInEndpoint, content, cancellationToken).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                    throw new StreamableAuthException("Empty response from Streamable API", ErrorCode.EmptyResponse);

                //I might Replace this with error model in future
                ThrowIfErrorResponse(responseContent);

                return DeserializeAuthResponse(responseContent);
            }
            catch (HttpRequestException ex)
            {
                throw new StreamableException("Network error occurred during authentication", ErrorCode.NetworkError, ex);
            }
        }

        /// <summary>
        /// (Internal use only) Deserializes the authentication response from Streamable API
        /// </summary>
        /// <remarks>
        /// This method is intended for internal use within the library and should not be called directly.
        /// </remarks>
        /// <param name="response">JSON response string from the API</param>
        /// <returns>Deserialized authentication result</returns>
        private static AuthenticatedUser DeserializeAuthResponse(string response)
        {
            var authResult = new AuthenticatedUser();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            authResult = JsonSerializer.Deserialize<AuthenticatedUser>(response, options);

            if (authResult is null)
            {
                throw new StreamableAuthException("Failed to deserialize authentication response", ErrorCode.InvalidResponse);
            }

            return authResult;
        }

        /// <summary>
        /// Logs out the current session (Not Implemented)
        /// </summary>
        /// <remarks>
        /// This method is currently not implemented in the Streamable API.
        /// </remarks>
        /// <param name="client">HttpClient instance to use for the request</param>
        /// <exception cref="ArgumentNullException">Thrown when client is null</exception>
        /// <exception cref="NotImplementedException">Always thrown as this operation is not supported</exception>
        public Task LogoutAsync(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            throw new NotImplementedException();
        }

        private static void ThrowIfErrorResponse(string responseContent)
        {
            try
            {
                var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);

                if (responseData?.TryGetValue("error", out var error) == true)
                {
                    throw new StreamableAuthException($"Authentication failed: {error}", ErrorCode.AuthenticationFailed);
                }
            }
            catch (JsonException jsonEx)
            {
                throw new StreamableAuthException("Authentication failed: Invalid response format", ErrorCode.InvalidResponse, jsonEx);
            }
        }

    }


}

