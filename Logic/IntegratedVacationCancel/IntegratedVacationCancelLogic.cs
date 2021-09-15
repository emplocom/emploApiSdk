using System;
using System.Collections.Generic;
using System.Linq;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Extensions;
using EmploApiSDK.Logger;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using EmploApiSDK.ApiModels.Vacations.IntegratedVacationCancel;

namespace EmploApiSDK.Logic.IntegratedVacationCancel
{
    public class IntegratedVacationCancelLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public IntegratedVacationCancelLogic(ILogger logger)
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


        public async Task<CancelIntegratedVactionsResponseModel> CancelVacations(List<int> vacationsIds, string comment)
        {
            if (!vacationsIds.Any())
            {
                return null;
            }

            CancelIntegratedVactionsResponseModel finalResult = new CancelIntegratedVactionsResponseModel();
            int chunkSize = AppSettingsConfigurationProvider.GetChunkSize();
            _logger.WriteLine(String.Format("Deleting external vacations (in chunks in size of {0})",
                chunkSize));

            foreach (var chunk in vacationsIds.Chunk(chunkSize))
            {
                var cancelIntegratedVacationsRequestModel = new CancelIntegratedVactionsRequestModel()
                {
                    VacationIds = chunk.ToList(),
                    Comment = comment
                };
                var serializedData = JsonConvert.SerializeObject(cancelIntegratedVacationsRequestModel);
                try
                {
                    var deleteIntegratedVactionsResponseModel =
                        await _apiClient.SendPostAsync<CancelIntegratedVactionsResponseModel>(serializedData,
                            _apiConfiguration.EmploUrl + "/" + _apiConfiguration.ApiPath + "/IntegratedVacations/CancelIntegratedVacations");

                    if (deleteIntegratedVactionsResponseModel.Status == CancelIntegrationVacationStatusEnum.Failed)
                    {
                        _logger.WriteLine($"Canceling vacation requests with ids {JsonConvert.SerializeObject(vacationsIds)} failed");
                    }

                    finalResult.Message += deleteIntegratedVactionsResponseModel.Message;
                    if (finalResult.Status == CancelIntegrationVacationStatusEnum.Success)
                    {
                        finalResult.Status = deleteIntegratedVactionsResponseModel.Status;
                    }
                    finalResult.VacationsCancellationStatuses =
                        finalResult.VacationsCancellationStatuses
                        .Concat(deleteIntegratedVactionsResponseModel.VacationsCancellationStatuses)
                        .ToDictionary(k => k.Key, v => v.Value);
                }
                catch (EmploApiClientFatalException e)
                {
                    _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);

                }
            }

            return finalResult;
        }
    }

}