using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EmploApiSDK.Logger;
using EmploApiSDK.Models;
using System.Threading.Tasks;
using CogisoftConnector.Logic;

namespace EmploApiSDK
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

        public T SendPost<T>(string json, string url)
        {
            return Send<T>(json, url).Result;
        }

        public async Task<T> SendPostAsync<T>(string json, string url)
        {
            return await Send<T>(json, url);
        }

        private async Task<T> Send<T>(string json, string url)
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

                    response = await PostAsync(client, uri, stringContent);
                    var result = await ReadAsStringAsync(response.Content);

                    if(response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                    else
                    {
                        _logger.WriteLine(String.Format("Response: {0} {1} {2}", (int)response.StatusCode, response.StatusCode, response.ReasonPhrase));
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
                _logger.WriteLine(String.Format("WebException, Response: {0} {1}", (int)code, code));

                var responseStream = ex.Response.GetResponseStream();
                if(responseStream != null)
                {
                    using(var sr = new StreamReader(responseStream))
                    {
                        var error = sr.ReadToEnd();
                        _logger.WriteLine(String.Format("Http status code: {0}, response: {1}", response != null ? response.StatusCode.ToString() : "?", error));
                    }
                }
                else
                {
                    _logger.WriteLine(ex.ToString());
                }
            }
            catch(TaskCanceledException ex)
            {
                _logger.WriteLine("Possible timeout!");
                if(response != null)
                {
                    _logger.WriteLine("Response StatusCode: " + response.StatusCode);
                    if(!String.IsNullOrEmpty(response.ReasonPhrase))
                        _logger.WriteLine("Response ReasonPhrase: " + response.ReasonPhrase);
                    if(response.Content != null)
                        _logger.WriteLine("Response Content: " + await ReadAsStringAsync(response.Content));
                }
                _logger.WriteLine("Error details: " + ex);
            }
            catch(Exception ex)
            {
                _logger.WriteLine("Unexpected error occured!");
                if(response != null)
                {
                    _logger.WriteLine("Response StatusCode: " + response.StatusCode);
                    if(!String.IsNullOrEmpty(response.ReasonPhrase))
                        _logger.WriteLine("Response ReasonPhrase: " + response.ReasonPhrase);
                    if(response.Content != null)
                        _logger.WriteLine("Response Content: " + await ReadAsStringAsync(response.Content));
                }
                _logger.WriteLine("Error details: " + ex);
            }

            Environment.Exit(-1);
            return default(T);
        }

        private async Task<string> ReadAsStringAsync(HttpContent content)
        {
            return await content.ReadAsStringAsync();
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
                _logger.WriteLine("Login error:" + _token.Json);
                Environment.Exit(-1);
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
