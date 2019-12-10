using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Common;
using EmploApiSDK.ApiModels.Employees;

namespace EmploApiSDK.ApiModels.Vacations.ImportVacations
{
    public class ImportVacationResponseModel
    {
        public ImportStatusCode ImportStatusCode { get; set; }
        public string ImportId { get; set; }
        public List<ImportValidationSummaryRow> OperationResults { get; set; }


    }
}
