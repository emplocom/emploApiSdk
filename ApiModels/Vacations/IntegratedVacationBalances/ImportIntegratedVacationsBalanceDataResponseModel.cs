using System.Collections.Generic;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationBalances
{
    public enum ImportVacationDataStatusCode
    {
        Ok,
        NoChange,
        Warning,
        Error
    }

    public class ImportIntegratedVacationsBalanceDataResponseModel
    {
        public ImportIntegratedVacationsBalanceDataResponseModel()
        {
            resultRows = new List<ImportIntegratedVacationsBalanceDataResponseRow>();
        }

        public List<ImportIntegratedVacationsBalanceDataResponseRow> resultRows;
    }

    public class ImportIntegratedVacationsBalanceDataResponseRow
    {
        public string ExternalEmployeeId;
        public ImportVacationDataStatusCode OperationStatus;
        public string Message = string.Empty;
    }
}
