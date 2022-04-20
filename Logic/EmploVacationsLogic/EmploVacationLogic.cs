using EmploApiSDK.ApiModels.Vacations.EmploVacations;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploApiSDK.Logic.EmploVacationsLogic
{
    public class EmploVacationLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public EmploVacationLogic(ILogger logger)
        {
            _logger = logger;

            _apiConfiguration = new ApiConfiguration()
            {
                EmploUrl = AppSettingsConfigurationProvider.GetEmploUrl(),
                ApiPath = AppSettingsConfigurationProvider.GetApiPath(),
                Login = AppSettingsConfigurationProvider.GetEmploLogin(),
                Password = AppSettingsConfigurationProvider.GetEmploPassword(),
            };
            _apiClient = new ApiClient(_logger, _apiConfiguration);
        }

        public async Task<List<EmploVacationsDataResponseModel>> GetEmploVacations(string employeeId, string integratedSystemId)
        {
            _logger.WriteLine(String.Format("Get employee with ID {0} vacation from emplo.", employeeId));
            List<EmploVacationsDataResponseModel> result = new List<EmploVacationsDataResponseModel>();
            DateTime lastVacationDate = new DateTime(DateTime.Now.Year - 1, 1, 1);
            var page = 1;
            var pageSize = 50;

            while (true)
            {
                try
                {
                    var emploVacationsDataResponseModel =
                                        await _apiClient.SendGetAsync<EmploVacationsDataResponseModelListing>(
                                            _apiConfiguration.GetEmployeeVacations.Replace("{employeeId}", employeeId.ToString()).Replace("{integratedSystemId}", integratedSystemId)
                                            .Replace("{page}", page.ToString()).Replace("{startDate}", lastVacationDate.ToString("s")));

                    result.AddRange(emploVacationsDataResponseModel.List);
                    if (emploVacationsDataResponseModel.List.Count() < pageSize)
                    {
                        break;
                    }
                    page++;
                }
                catch (EmploApiClientFatalException e)
                {
                    _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);
                    break;
                }
            };

            return result;
        }
    }
}
