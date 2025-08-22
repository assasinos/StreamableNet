using StreamableNet.Auth;
using StreamableNet.Clients;
using StreamableNet.Consts;

namespace StreamableNet
{
    public class StreamableClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthProvider _authProvider;
        private readonly StreamableOptions _options;
        private readonly bool _disposeHttpClient;

        public UserClient User { get; private set; }
        public VideoClient Video { get; private set; }
        public UploadClient Upload { get; private set; }


        public StreamableClient(IAuthProvider authProvider, HttpClient httpClient, StreamableOptions? streamableOptions = null)
        {
            _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = streamableOptions ?? new StreamableOptions();

            InitializeClients();
        }

        public StreamableClient(IAuthProvider authProvider, StreamableOptions? streamableOptions = null)
        {
            _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            _httpClient = new();
            _disposeHttpClient = true;
            _options = streamableOptions ?? new StreamableOptions();

            InitializeClients();
        }

        private void InitializeClients()
        {
            User = new UserClient(_httpClient, _authProvider, _options);
            Video = new VideoClient(_httpClient, _options);
            Upload = new UploadClient(_httpClient, _options);
        }


        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                _httpClient?.Dispose();
            }
        }

    }
}

