using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.Logger;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmploApiSDK.Client
{
    public class ApiClient
    {
        private readonly ILogger _logger;
        private readonly ApiConfiguration _apiConfiguration;
        private readonly AuthorizationManager _authorizationManager;
        private TokenResponse _token;

        public ApiClient(ILogger logger, ApiConfiguration apiConfiguration)
        {
            _logger = logger;
            this._apiConfiguration = apiConfiguration;
            _authorizationManager = new AuthorizationManager(logger, apiConfiguration);
        }

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error has occurred during emplo API call </exception>
        public T SendGet<T>(string url)
        {
            return Send<T>(string.Empty, url, HttpMethod.Get).Result;
        }

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error has occurred during emplo API call </exception>
        public async Task<T> SendGetAsync<T>(string url)
        {
            return await Send<T>(string.Empty, url, HttpMethod.Get);
        }

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error has occurred during emplo API call </exception>
        public T SendPost<T>(string json, string url)
        {
            return Send<T>(json, url, HttpMethod.Post).Result;
        }

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error has occurred during emplo API call </exception>
        public async Task<T> SendPostAsync<T>(string json, string url)
        {
            return await Send<T>(json, url, HttpMethod.Post);
        }

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error, requiring request abortion, has occurred </exception>
        private async Task<T> Send<T>(string json, string url, HttpMethod httpMethod)
        {
            EnsureValidToken();
            _logger.WriteLine("Token: " + _token.Json);

            HttpResponseMessage response = null;
            try
            {
                using(var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    var client = HttpClientProvider.HttpClient;

                    var uri = new Uri(url);
                    _logger.WriteLine("Calling API " + uri);
                    _logger.WriteLine("Request content: " + json);

                    if (httpMethod == HttpMethod.Get)
                    {
                        response = await GetAsync(client, uri);
                    }
                    else
                    {
                        response = await PostAsync(client, uri, stringContent);
                    }

                    var result = await ReadAsStringAsync(response.Content);

                    if(response.IsSuccessStatusCode)
                    {
                        if (result != null && !result.Equals(string.Empty))
                        {
                            _logger.WriteLine($"API call successful, http response code: {(int)response.StatusCode}, http response content: {result}");
                            return JsonConvert.DeserializeObject<T>(result);
                        }
                        else
                        {
                            _logger.WriteLine($"API call successful, http response code: {(int)response.StatusCode}");
                            return default(T);
                        }
                    }
                    else
                    {
                        _logger.WriteLine(String.Format("Response: {0} {1} {2}", (int)response.StatusCode, response.StatusCode, response.ReasonPhrase), LogLevelEnum.Error);
                        if(response.Content != null)
                        {
                            _logger.WriteLine(await ReadAsStringAsync(response.Content));
                        }
                    }
                }
            }
            catch(WebException ex)
            {
                var code = ((HttpWebResponse)ex.Response).StatusCode;
                _logger.WriteLine($"WebException, Response: {(int) code} {code}", LogLevelEnum.Error);

                var responseStream = ex.Response.GetResponseStream();
                if(responseStream != null)
                {
                    using(var sr = new StreamReader(responseStream))
                    {
                        var error = sr.ReadToEnd();
                        _logger.WriteLine(
                            $"Http status code: {(response != null ? response.StatusCode.ToString() : "?")}, response: {error}", LogLevelEnum.Error);
                    }
                }
                else
                {
                    _logger.WriteLine(ex.ToString(), LogLevelEnum.Error);
                }
            }
            catch(TaskCanceledException ex)
            {
                _logger.WriteLine("Possible timeout!", LogLevelEnum.Error);
                if(response != null)
                {
                    _logger.WriteLine("Response StatusCode: " + response.StatusCode, LogLevelEnum.Error);
                    if(!String.IsNullOrEmpty(response.ReasonPhrase))
                        _logger.WriteLine("Response ReasonPhrase: " + response.ReasonPhrase, LogLevelEnum.Error);
                    if(response.Content != null)
                        _logger.WriteLine("Response Content: " + await ReadAsStringAsync(response.Content), LogLevelEnum.Error);
                }
                _logger.WriteLine("Error details: " + ex, LogLevelEnum.Error);
            }
            catch(Exception ex)
            {
                _logger.WriteLine("Unexpected error occured!", LogLevelEnum.Error);
                if(response != null)
                {
                    _logger.WriteLine("Response StatusCode: " + response.StatusCode, LogLevelEnum.Error);
                    if(!String.IsNullOrEmpty(response.ReasonPhrase))
                        _logger.WriteLine("Response ReasonPhrase: " + response.ReasonPhrase, LogLevelEnum.Error);
                    if(response.Content != null)
                        _logger.WriteLine("Response Content: " + await ReadAsStringAsync(response.Content), LogLevelEnum.Error);
                }
                _logger.WriteLine("Error details: " + ex, LogLevelEnum.Error);
            }

            throw new EmploApiClientFatalException($"A fatal error has occurred during emplo API request processing.");
        }

        private async Task<string> ReadAsStringAsync(HttpContent content)
        {
            return await content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> GetAsync(HttpClient client, Uri uri)
        {
            HttpRequestMessage message = new HttpRequestMessage() { RequestUri = uri, Method = HttpMethod.Get };
            message.Headers.Add("Authorization", "Bearer " + _token.AccessToken);

            return await client.SendAsync(message);
        }

        private async Task<HttpResponseMessage> PostAsync(HttpClient client, Uri uri, StringContent stringContent)
        {
            HttpRequestMessage message = new HttpRequestMessage() {RequestUri = uri, Content = stringContent, Method = HttpMethod.Post };
            message.Headers.Add("Authorization", "Bearer " + _token.AccessToken);

            return await client.SendAsync(message);
        }

        private void EnsureValidToken()
        {
            if (_token == null)
            {
                LogIn();
            }
            else if (_token.AccessToken.Contains("."))
            {
                var parts = _token.AccessToken.Split('.');
                var claims = parts[1];
                var decodedClaims = JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims)));
                var expirationDate = UnixTimeStampToDateTime(Convert.ToInt32(decodedClaims["exp"]));
                if (DateTime.UtcNow.AddMinutes(10) >= expirationDate)
                {
                    _token = _authorizationManager.RefreshToken(_token.RefreshToken);                    
                }
            }
        }

        private void LogIn()
        {
            var login = _apiConfiguration.Login;
            var password = _apiConfiguration.Password;

            _token = _authorizationManager.RequestToken(login, password);

            if (_token.AccessToken == null)
            {
                _logger.WriteLine("Login error:" + _token.Json, LogLevelEnum.Error);
                throw new EmploApiClientFatalException($"A fatal error has occurred during emplo API LogIn.");
            }
            else
            {
                _logger.WriteLine("Login to emplo was successful");
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(unixTimeStamp);
            return date;
        }
    }
}
