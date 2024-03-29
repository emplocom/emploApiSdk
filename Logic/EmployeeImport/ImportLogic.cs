﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Common;
using EmploApiSDK.ApiModels.Employees;
using EmploApiSDK.Client;
using EmploApiSDK.Logger;
using Newtonsoft.Json;
using ImportStatusCode = EmploApiSDK.ApiModels.Common.ImportStatusCode;
using ImportStatuses = EmploApiSDK.ApiModels.Common.ImportStatuses;

namespace EmploApiSDK.Logic.EmployeeImport
{
    public class ImportLogic
    {
        private readonly ILogger _logger;
        private readonly ApiClient _apiClient;
        private readonly ApiConfiguration _apiConfiguration;

        public ImportLogic(ILogger logger)
        {
            _logger = logger;

            _apiConfiguration = new ApiConfiguration()
            {
                EmploUrl = ConfigurationManager.AppSettings["EmploUrl"],
                ApiPath = ConfigurationManager.AppSettings["ApiPath"] ?? "apiv2",
                Login = ConfigurationManager.AppSettings["Login"],
                Password = ConfigurationManager.AppSettings["Password"],
            };

            _apiClient = new ApiClient(_logger, _apiConfiguration);
        }

        public async Task<int> ImportEmployees(ImportUsersRequestModel importUsersRequestModel)
        {
            try
            {
                int chunkSize = GetChunkSize();
                _logger.WriteLine(String.Format("Sending employee data to emplo (in chunks in size of {0})",
                    chunkSize));

                // first, send data without superiors
                foreach (var chunk in Chunk(importUsersRequestModel.Rows, chunkSize))
                {
                    var importUsersRequestModelChunk = new ImportUsersRequestModel(importUsersRequestModel.Mode,
                        importUsersRequestModel.RequireRegistrationForNewEmployees)
                    {
                        ImportId = importUsersRequestModel.ImportId,
                        Rows = chunk.ToList()
                    };
                    var serializedData = JsonConvert.SerializeObject(importUsersRequestModelChunk);
                    var importValidationSummary =
                        await _apiClient.SendPostAsync<ImportUsersResponseModel>(serializedData,
                            _apiConfiguration.ImportUsersUrl);
                    if (importValidationSummary.ImportStatusCode != ImportStatusCode.Ok)
                    {
                        _logger.WriteLine(
                            "Import action returned error status: " + importValidationSummary.ImportStatusCode,
                            LogLevelEnum.Error);
                        return -1;
                    }
                    importUsersRequestModel.ImportId = importValidationSummary.ImportId;
                    SaveImportValidationSummaryLog(importValidationSummary);
                }

                if (importUsersRequestModel.Rows.Any())
                {
                    _logger.WriteLine("Finishing import...");
                    FinishImportRequestModel requestModel =
                        new FinishImportRequestModel(ConfigurationManager.AppSettings["BlockSkippedUsers"]);
                    requestModel.ImportId = importUsersRequestModel.ImportId;
                    var serializedData = JsonConvert.SerializeObject(requestModel);
                    var finishImportResponse =
                        await _apiClient.SendPostAsync<FinishImportResponseModel>(serializedData,
                            _apiConfiguration.FinishImportUrl);
                    if (finishImportResponse.ImportStatusCode != ImportStatusCode.Ok)
                    {
                        _logger.WriteLine(
                            "FinishImport action returned error status: " + finishImportResponse.ImportStatusCode,
                            LogLevelEnum.Error);
                        return -1;
                    }
                    else
                    {
                        if (finishImportResponse.BlockedUserIds != null && finishImportResponse.BlockedUserIds.Any())
                        {
                            _logger.WriteLine("Blocked user id's: " +
                                              String.Join(", ", finishImportResponse.BlockedUserIds));
                        }

                        if (finishImportResponse.UpdateUnitResults != null &&
                            finishImportResponse.UpdateUnitResults.Any())
                        {
                            _logger.WriteLine("Units tree was updated:");
                            foreach (var message in finishImportResponse.UpdateUnitResults)
                            {
                                if (message.IsError)
                                {
                                    _logger.WriteLine(String.Format("Unit update error: {0}", message.Message),
                                        LogLevelEnum.Error);
                                }
                                else
                                {
                                    _logger.WriteLine(String.Format(
                                        "Unit updated: unit {0} was updated, old parent={1}, new parent={2}, message: {3}",
                                        message.UpdatedUnitId, message.OldParentId, message.NewParentId,
                                        message.Message));
                                }
                            }
                        }

                        if (bool.TryParse(ConfigurationManager.AppSettings["DismissBlockedUsers"], out var dismissBlockedUser) && dismissBlockedUser)
                        {
                            var actionResult = await DismissBlockedUsers(importUsersRequestModel.ImportId);
                            if (actionResult != 0)
                            {
                                return actionResult;
                            }
                        }
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

        private async Task<int> DismissBlockedUsers(string importId)
        {
            var serializedData = JsonConvert.SerializeObject(importId);
            var dismissBlockedUsersReponse =
                await _apiClient.SendPostAsync<DismissBlockedUsersResponseModel>(serializedData,
                    _apiConfiguration.DismissBlockedUsersUrl);

            if (dismissBlockedUsersReponse.Rows.Any(d => d.Message != null && d.Message.Any()))
            {
                foreach (var responseRow in dismissBlockedUsersReponse.Rows)
                {
                    if (responseRow.IsSuccessfull)
                    {
                        _logger.WriteLine($"Dismiss user {responseRow.EmployeeId} has been skipped: {responseRow.Message}");
                    }
                    else
                    {
                        _logger.WriteLine($"Dismiss user {responseRow.EmployeeId} has finished with error: {responseRow.Message}");
                    }
                }
            }
            else
            {
                _logger.WriteLine("Dismiss blocked users has finished successfully");
            }
            if (bool.TryParse(ConfigurationManager.AppSettings["PermanentRemoveBlockedUsers"], out var permanentRemoveBlockedUsers) && permanentRemoveBlockedUsers)
            {
                var actionResult = await PermanentRemoveBlockedUsers(importId);
                if (actionResult != 0)
                {
                    return actionResult;
                }
            }

            return 0;
        }

        private async Task<int> PermanentRemoveBlockedUsers(string importId)
        {
            var serializedData = JsonConvert.SerializeObject(importId);
            var permanentRemoveBlockedUsersResponse =
                await _apiClient.SendPostAsync<PermanentRemoveBlockedUsersResponseModel>(serializedData,
                    _apiConfiguration.PermanentRemoveBlockedUsersUrl);

            if (permanentRemoveBlockedUsersResponse.Rows.Any(d => d.Message != null && d.Message.Any()))
            {
                foreach (var responseRow in permanentRemoveBlockedUsersResponse.Rows)
                {
                    if (responseRow.IsSuccessfull)
                    {
                        _logger.WriteLine($"Permanent remove user {responseRow.EmployeeId} has been skipped: {responseRow.Message}");
                    }
                    else
                    {
                        _logger.WriteLine($"Permanent remove user {responseRow.EmployeeId} has finished with error: {responseRow.Message}");
                    }
                }
            }
            else
            {
                _logger.WriteLine("Permanent remove blocked users has finished successfully");
            }

            return 0;
        }

        private IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        private void SaveImportValidationSummaryLog(ImportUsersResponseModel importValidationSummary)
        {
            if (!importValidationSummary.OperationResults.Any())
            {
                _logger.WriteLine("Result is empty.");
            }

            foreach (var result in importValidationSummary.OperationResults)
            {
                string employeeHeader = String.Format("{0} (id={1})", result.Employee, result.EmployeeId);

                if (result.StatusCode == ImportStatuses.Skipped)
                {
                    _logger.WriteLine(String.Format("{0} SKIPPED - {1}", employeeHeader, result.Message));
                    continue;
                }

                if (result.StatusCode != ImportStatuses.Ok)
                {
                    string error = String.Format("Status: {0}, ErrorColumns: {1}, Message: {2}", result.StatusCode, String.Join(",", result.ErrorColumns), result.Message);
                    _logger.WriteLine(String.Format("{0} ERROR - {1}", employeeHeader, error), LogLevelEnum.Error);
                    continue;
                }

                string changedColumns = String.Join(", ", result.ChangedColumns);
                if (result.Created)
                {
                    _logger.WriteLine(String.Format("{0} CREATED - Changed columns: {1}", employeeHeader, changedColumns));
                }
                else
                {
                    if (changedColumns.Any())
                    {
                        _logger.WriteLine(String.Format("{0} UPDATED - Updated columns: {1}", employeeHeader, changedColumns));
                    }
                    else
                    {
                        _logger.WriteLine(String.Format("{0} NO CHANGES", employeeHeader));
                    }
                }
            }
        }

        private int GetChunkSize()
        {
            string sizeString = ConfigurationManager.AppSettings["ChunkSize"] ?? "";
            int size;
            if (Int32.TryParse(sizeString, out size))
                return size;
            return 5;
        }
    }
}
