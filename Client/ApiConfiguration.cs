namespace EmploApiSDK.Models
{
    public class ApiConfiguration
    {
        public string EmploUrl { get; set; }
        public string ApiPath { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string ImportUsersUrl => EmploUrl + "/" + ApiPath + "/Users/Import";
        public string FinishImportUrl => EmploUrl + "/" + ApiPath + "/Users/FinishImport";
        public string BlockUserUrl => EmploUrl + "/" + ApiPath + "/Users/Block";
        public string TokenEndpoint => EmploUrl + "/identity/connect/token";
        public string ImportIntegratedVacationsBalanceDataUrl =>
            EmploUrl + "/" + ApiPath + "/Vacations/ImportVacationsBalanceData";
    }
}
