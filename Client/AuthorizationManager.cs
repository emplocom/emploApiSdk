using EmploApiSDK.Models;
using EmploApiSDK.Logger;
using IdentityModel.Client;

namespace EmploApiSDK
{
    internal class AuthorizationManager
    {
        private readonly ILogger _logger;
        private readonly ApiConfiguration _apiConfiguration;
        static TokenClient _companyTokenClient;

        public AuthorizationManager(ILogger logger, ApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;
            _logger = logger;

            var tokenendpoint = _apiConfiguration.TokenEndpoint;
            _companyTokenClient = new TokenClient(tokenendpoint, "ResourceOwnerClient", "6D359719-149A-4011-91D4-01CBA687DBBF");

            var tokenResponse = _companyTokenClient.RequestClientCredentialsAsync().Result;
        }

        public TokenResponse RequestToken(string login, string password)
        {
            _logger.WriteLine("Sending authorization request to " + _apiConfiguration.TokenEndpoint);
            return _companyTokenClient.RequestResourceOwnerPasswordAsync(login, password,
                "read write offline_access").Result;
        }

        public TokenResponse RefreshToken(string refreshToken)
        {
            _logger.WriteLine("Sending refresh token request to " + _apiConfiguration.TokenEndpoint);
            return _companyTokenClient.RequestRefreshTokenAsync(refreshToken).Result;
        }
    }
}
