using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Employees
{
    public class DismissBlockedUsersRequestModel
    {
        public DismissBlockedUsersRequestModel(string importId)
        {
            ImportId = importId;
        }

        /// <summary>
        /// Id of the import whick is related to action
        /// </summary>
        public string ImportId { get; set; }
    }
}
