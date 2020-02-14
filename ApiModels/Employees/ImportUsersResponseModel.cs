using System;
using System.Collections.Generic;
using EmploApiSDK.ApiModels.Common;

namespace EmploApiSDK.ApiModels.Employees
{
    public class ImportUsersResponseModel
    {
        public EmploApiSDK.ApiModels.Common.ImportStatusCode ImportStatusCode { get; set; }
        public string ImportId { get; set; }
        public List<EmploApiSDK.ApiModels.Common.ImportValidationSummaryRow> OperationResults { get; set; }
    }

    [Obsolete("Only for backward compability, DO NOT USE")]
    public enum ImportStatuses
    {
        Ok,
        MissingData,
        InvalidData,
        NotImplemented,
        ObjectAlreadyExists,
        Error,
        Skipped
    }

    [Obsolete("Only for backward compability, DO NOT USE")]
    public class ImportValidationSummaryRow
    {
        public ImportStatuses StatusCode { get; set; }
        public int? EmployeeId { get; set; }
        public string Employee { get; set; }
        public List<string> ErrorColumns { get; set; }
        public List<string> ChangedColumns { get; set; }
        public bool Created { get; set; }
        public string Message { get; set; }
    }

    [Obsolete("Only for backward compability, DO NOT USE")]
    public enum ImportStatusCode
    {
        Ok,
        WrongImportId,
        ImportIsFinished
    }


}
