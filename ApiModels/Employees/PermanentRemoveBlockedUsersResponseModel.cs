using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Employees
{
    public class PermanentRemoveBlockedUsersResponseModel
    {
        public List<PermanentRemoveEmployeeRow> Rows { get; set; }
    }

    public class PermanentRemoveEmployeeRow
    {
        public int EmployeeId { get; set; }
        public string ExternalEmployeeId { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }

    }

}
