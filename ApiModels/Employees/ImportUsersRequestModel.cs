using System.Collections.Generic;

namespace EmploApiSDK.ApiModels.Employees
{
    public class ImportUsersRequestModel
    {
        public ImportUsersRequestModel(string importMode, string requireRegistrationForNewEmployees)
        {
            Mode = importMode;
            RequireRegistrationForNewEmployees = requireRegistrationForNewEmployees;
            Rows = new List<UserDataRow>();
        }

        public string ImportId { get; set; }

        /// <summary>
        /// Data to import
        /// </summary>
        public List<UserDataRow> Rows { get; set; }

        /// <summary>
        /// Indicates if data should be created, inserted or both
        /// </summary>
        public string Mode { get; set; }

        public string RequireRegistrationForNewEmployees { get; set; }
    }
}
