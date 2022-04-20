using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Employees
{
    public class DismissBlockedUsersResponseModel
    {
        public List<DismissRow> Rows { get; set; }
    }

    public class DismissRow
    {
        public int EmployeeId { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Message { get; set; }
    }
}
