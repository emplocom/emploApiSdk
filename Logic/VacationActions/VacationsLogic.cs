using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Common;
using EmploApiSDK.ApiModels.Vacations.ImportVacations;
using EmploApiSDK.ApiModels.Vacations.Actions;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Extensions;
using EmploApiSDK.Logger;
using Newtonsoft.Json;
using System.Net.Http;

namespace EmploApiSDK.Logic.VacationActions
{
    public class VacationsLogic 
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public VacationsLogic(ILogger logger, ApiClient apiClient = null)
        {
            _logger = logger;

            _apiConfiguration = new ApiConfiguration()
            {
                EmploUrl = AppSettingsConfigurationProvider.GetEmploUrl(),
                ApiPath = AppSettingsConfigurationProvider.GetApiPath(),
                Login = AppSettingsConfigurationProvider.GetEmploLogin(),
                Password = AppSettingsConfigurationProvider.GetEmploPassword(),
            };

            if (apiClient == null)
            {
                _apiClient = new ApiClient(_logger, _apiConfiguration);
            }
            else
            {
                _apiClient = apiClient;
            }
        }


        public async Task PostVacationComment(string comment, int vacationRequestId)
        {
            await _apiClient.SendPostAsync<VacationCommentResponse>("{}",
                            _apiConfiguration.PostCommentToVacationUrl.Replace("{Id}",vacationRequestId.ToString()));

        }

        public async Task RejectVacation(int vacationRequestId)
        {
            await _apiClient.SendPostAsync<HttpResponseMessage>("{}",
                            _apiConfiguration.RejectVacationUrl.Replace("{Id}", vacationRequestId.ToString()));

        }
        
    }
}
