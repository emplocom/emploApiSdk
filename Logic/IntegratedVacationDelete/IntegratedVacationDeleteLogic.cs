using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Common;
using EmploApiSDK.ApiModels.Vacations.ImportVacations;
using EmploApiSDK.ApiModels.Vacations.IntegratedVacationDelete;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Extensions;
using EmploApiSDK.Logger;
using Newtonsoft.Json;

namespace EmploApiSDK.Logic.IntegratedVacationDelete
{
    public class IntegratedVacationDeleteLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public IntegratedVacationDeleteLogic(ILogger logger, ApiClient apiClient = null)
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


        public async Task<DeleteIntegratedVactionsResponseModel>  DeleteVacations(List<string> externalVacationsId, int externalSystemId)
        {
            DeleteIntegratedVactionsResponseModel finalResult = new DeleteIntegratedVactionsResponseModel();
            int chunkSize = AppSettingsConfigurationProvider.GetChunkSize();
            _logger.WriteLine(String.Format("Deleting external vacations (in chunks in size of {0})",
                chunkSize));

            foreach (var chunk in externalVacationsId.Chunk(chunkSize))
            {
                var deleteIntegratedVacationsRequestModel = new DeleteIntegratedVactionsRequestModel()
                {
                    ExternalVacationIds = chunk.ToList(),
                    ExternalSystemId = externalSystemId
                };
                var serializedData = JsonConvert.SerializeObject(deleteIntegratedVacationsRequestModel);
                try
                {
                    var deleteIntegratedVactionsResponseModel =
                        await _apiClient.SendPostAsync<DeleteIntegratedVactionsResponseModel>(serializedData,
                            _apiConfiguration.DeleteIntegratedVacations);

                    if (deleteIntegratedVactionsResponseModel.Operations != null &&
                            deleteIntegratedVactionsResponseModel.Operations.Any())
                    {
                        finalResult.Operations.AddRange(deleteIntegratedVactionsResponseModel.Operations);
                        SaveDeleteChunkRow(deleteIntegratedVactionsResponseModel.Operations);
                    }
                }
                catch (EmploApiClientFatalException e)
                {
                    _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);

                }
            }

            return finalResult;
        }

        private void SaveDeleteChunkRow(List<DeleteIntegratedVactionsResponseRow> operations)
        {
            var vacationsDeleted = string.Join(",",operations.Where(x => x.Status == DeleteIntegrationVacationStatus.Success).Select(x => x.ExternalVacationId));
            _logger.WriteLine($"Deleted rows: ({vacationsDeleted})");

            var vacationsWithProblems = operations.Where(x => x.Status == DeleteIntegrationVacationStatus.Failed);
            foreach (var deleteIntegratedVactionsResponseRow in vacationsWithProblems)
            {
                _logger.WriteLine(
                    $"Problem with externalVacationId = {deleteIntegratedVactionsResponseRow.ExternalVacationId}, Message = '{deleteIntegratedVactionsResponseRow.Message}'");
            }

        }
    }
}
