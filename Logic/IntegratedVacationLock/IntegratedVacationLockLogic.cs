using EmploApiSDK.ApiModels.Vacations.IntegratedVacationLock;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Extensions;
using EmploApiSDK.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EmploApiSDK.Logic.IntegratedVacationLock
{
    public class IntegratedVacationLockLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public IntegratedVacationLockLogic(ILogger logger)
        {
            _logger = logger;

            _apiConfiguration = new ApiConfiguration()
            {
                EmploUrl = ConfigurationManager.AppSettings["EmploUrl"],
                ApiPath = ConfigurationManager.AppSettings["ApiPath"] ?? "apiv2",
                Login = ConfigurationManager.AppSettings["Login"],
                Password = ConfigurationManager.AppSettings["Password"]
            };

            _apiClient = new ApiClient(_logger, _apiConfiguration);
        }

        public async Task<bool> LockVacations(List<int> vacationsIds)
        {
            if (!vacationsIds.Any())
            {
                return true;
            }

            bool finalResult = true;

            int chunkSize = AppSettingsConfigurationProvider.GetChunkSize();
            _logger.WriteLine(string.Format("Deleting external vacations (in chunks in size of {0})",
                chunkSize));

            foreach (var chunk in vacationsIds.Chunk(chunkSize))
            {
                var serializedData = JsonConvert.SerializeObject(chunk.ToList());
                try
                {
                    var lockIntegratedVactionsResponseModel =
                        await _apiClient.SendPostAsync<LockIntegratedVactionsResponseModel>(serializedData,
                            _apiConfiguration.EmploUrl + "/" + _apiConfiguration.ApiPath + "/Vacations/LockVacations");

                    if (!lockIntegratedVactionsResponseModel.Success)
                    {
                        _logger.WriteLine($"Canceling vacation requests with ids {JsonConvert.SerializeObject(vacationsIds)} failed: {lockIntegratedVactionsResponseModel.Message}");
                    }

                    finalResult &= lockIntegratedVactionsResponseModel.Success;
                }
                catch (EmploApiClientFatalException e)
                {
                    _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);
                    return false;
                }

            }
            return finalResult;
        }
    }
}
