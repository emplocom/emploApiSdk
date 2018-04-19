namespace EmploApiSDK.ApiModels.Employees
{
    public class FinishImportRequestModel
    {
        public FinishImportRequestModel(string blockSkippedUsers)
        {
            BlockSkippedUsers = blockSkippedUsers;
        }

        /// <summary>
        /// Id of the import which is being finished
        /// </summary>
        public string ImportId { get; set; }

        /// <summary>
        /// If set to true, all the users which were not present in the given import will be blocked
        /// </summary>
        public string BlockSkippedUsers { get; set; }  
    }
}
