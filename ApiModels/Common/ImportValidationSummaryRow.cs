using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Common
{
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
}
