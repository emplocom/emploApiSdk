using System.Collections.Generic;
using EmploApiSDK.ApiModels.Common;

namespace EmploApiSDK.ApiModels.Employees
{
    public class ImportUsersResponseModel
    {
        public ImportStatusCode ImportStatusCode { get; set; }
        public string ImportId { get; set; }
        public List<ImportValidationSummaryRow> OperationResults { get; set; }
    }
}
