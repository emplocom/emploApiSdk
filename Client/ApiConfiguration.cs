namespace EmploApiSDK.Client
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
        public string CheckUserHasAccessUrl => EmploUrl + "/" + ApiPath + "/Users/HasAccess";
        public string TokenEndpoint => EmploUrl + "/identity/connect/token";
        public string ImportIntegratedVacationsBalanceDataUrl =>
            EmploUrl + "/" + ApiPath + "/IntegratedVacations/ImportVacationsBalanceData";

        public string ImportVacationsUrl => EmploUrl + "/" + ApiPath + "/Vacations/Import";
        public string FinishImportVacationsUrl => EmploUrl + "/" + ApiPath + "/Vacations/FinishImport";

        public string DeleteIntegratedVacations =>
            EmploUrl + "/" + ApiPath + "/IntegratedVacations/DeleteIntegratedVacations";

        public string PostCommentToVacationUrl => EmploUrl + "/" + ApiPath + "/Vacations/{Id}/Comments";
        public string RejectVacationUrl => EmploUrl + "/" + ApiPath + "/Vacations/{Id}/Reject";

        public string DismissBlockedUsersUrl => EmploUrl + "/" + ApiPath + "/Users/DismissBlockedUsers";
        public string PermanentRemoveBlockedUsersUrl => EmploUrl + "/" + ApiPath + "/Users/PermanentRemoveBlockedUsers";

    }
}
