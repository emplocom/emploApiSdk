using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Common;
using EmploApiSDK.ApiModels.Employees;
using EmploApiSDK.ApiModels.Vacations.ImportVacations;
using EmploApiSDK.Client;
using EmploApiSDK.Configuration;
using EmploApiSDK.Logger;
using Newtonsoft.Json;

namespace EmploApiSDK.Logic.VacationImport
{
    public class VacationImportLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public VacationImportLogic(ILogger logger)
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

        public async Task<int> ImportVacations(ImportVacationRequestModel importVacationRequestModel)
        {
            try
            {
                int chunkSize = AppSettingsConfigurationProvider.GetChunkSize();
                _logger.WriteLine(String.Format("Sending vacations data to emplo (in chunks in size of {0})",
                    chunkSize));

                foreach (var chunk in Chunk(importVacationRequestModel.Rows, chunkSize))
                {
                    var importVacationRequestModelChunk = new ImportVacationRequestModel()
                    {
                        ImportId = importVacationRequestModel.ImportId,
                        Rows = chunk.ToList()
                    };
                    var serializedData = JsonConvert.SerializeObject(importVacationRequestModelChunk);
                    var importValidationSummary =
                        await _apiClient.SendPostAsync<ImportVacationResponseModel>(serializedData,
                            _apiConfiguration.ImportVacationsUrl);
                    if (importValidationSummary.ImportStatusCode != ImportStatusCode.Ok)
                    {
                        _logger.WriteLine(
                            "Import action returned error status: " + importValidationSummary.ImportStatusCode,
                            LogLevelEnum.Error);
                        return -1;
                    }
                    importVacationRequestModel.ImportId = importValidationSummary.ImportId;
                    SaveImportValidationSummaryLog(importValidationSummary);
                }

                if (importVacationRequestModel.Rows.Any())
                {
                    _logger.WriteLine("Finishing import...");
                    FinishImportVacationRequestModel requestModel =
                        new FinishImportVacationRequestModel(importVacationRequestModel.ImportId);
                    var serializedData = JsonConvert.SerializeObject(requestModel);
                    var finishImportResponse =
                        await _apiClient.SendPostAsync<FinishImportVacationResponseModel>(serializedData,
                            _apiConfiguration.FinishImportVacationsUrl);
                    if (finishImportResponse.ImportStatusCode != ImportStatusCode.Ok)
                    {
                        _logger.WriteLine(
                            "FinishImport action returned error status: " + finishImportResponse.ImportStatusCode,
                            LogLevelEnum.Error);
                        return -1;
                    }
                }

                _logger.WriteLine("Import has finished successfully");
                return 0;
            }
            catch (EmploApiClientFatalException e)
            {
                _logger.WriteLine(ExceptionLoggingUtils.ExceptionAsString(e), LogLevelEnum.Error);
                return -1;
            }
        }

        private IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        private void SaveImportValidationSummaryLog(ImportVacationResponseModel importValidationSummary)
        {
            if (!importValidationSummary.OperationResults.Any())
            {
                _logger.WriteLine("Result is empty.");
            }

            foreach (var result in importValidationSummary.OperationResults)
            {
                if (result.StatusCode == ImportStatuses.Ok)
                {
                    _logger.WriteLine($"Result code: {result.StatusCode}");
                }
                else
                {
                    _logger.WriteLine($"Result code: {result.StatusCode}, Error message: {result.Message}");
                }
            }
        }


    }
}
