namespace EmploApiSDK.ApiModels.IntegratedVacations
{
    public enum ImportVacationDataStatusCode
    {
        Ok,
        Error
    }

    public class ImportIntegratedVacationsBalanceDataResponseModel
    {
        public ImportVacationDataStatusCode OperationStatus;
        public string ErrorMessage = string.Empty;
    }
}
