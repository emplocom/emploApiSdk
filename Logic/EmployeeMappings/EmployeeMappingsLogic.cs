using EmploApiSDK.ApiModels.Vacations.EmploVacations;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmploApiSDK.Logic.EmployeeMappings
{
    public class EmployeeMappingsLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public EmployeeMappingsLogic(ILogger logger)
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

        public async Task StartEmployeeMappingsSynchronization(string integratedSystemId)
        {
            _logger.WriteLine(String.Format("Employee mappings synchronization started."));
            try
            {
                await _apiClient.SendGetAsync<EmploVacationsDataResponseModelListing>(
                    _apiConfiguration.SynchronizeEmployeeMappings.Replace("{integratedSystemId}", integratedSystemId));
            }
            catch (EmploApiClientFatalException e)
            {
                _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);
            }
        }
    }
}
