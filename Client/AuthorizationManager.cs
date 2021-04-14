using EmploApiSDK.Logger;
using IdentityModel.Client;

namespace EmploApiSDK.Client
{
    internal class AuthorizationManager
    {
        private readonly ILogger _logger;
        private readonly ApiConfiguration _apiConfiguration;
        static TokenClient _companyTokenClient;
        private string tokenEndpoint;

        public AuthorizationManager(ILogger logger, ApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;
            _logger = logger;

            tokenEndpoint = _apiConfiguration.TokenEndpoint;
            

            //var tokenResponse = _companyTokenClient.RequestClientCredentialsAsync().Result;
        }

        private TokenClient GetTokenClient()
        {
            if(_companyTokenClient == null)
            {
                _companyTokenClient = _companyTokenClient = new TokenClient(tokenEndpoint, "ResourceOwnerClient", "6D359719-149A-4011-91D4-01CBA687DBBF");
            }
            return _companyTokenClient;

        }

        public TokenResponse RequestToken(string login, string password)
        {
            _logger.WriteLine("Sending authorization request to " + _apiConfiguration.TokenEndpoint);
            return GetTokenClient().RequestResourceOwnerPasswordAsync(login, password,
                "read write offline_access").Result;
        }

        public TokenResponse RefreshToken(string refreshToken)
        {
            _logger.WriteLine("Sending refresh token request to " + _apiConfiguration.TokenEndpoint);
            return GetTokenClient().RequestRefreshTokenAsync(refreshToken).Result;
        }
    }
}
