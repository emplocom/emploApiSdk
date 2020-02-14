using System.Collections.Generic;
using EmploApiSDK.ApiModels.Common;

namespace EmploApiSDK.ApiModels.Employees
{
    public class FinishImportResponseModel
    {
        public EmploApiSDK.ApiModels.Common.ImportStatusCode ImportStatusCode { get; set; }
        public List<int> BlockedUserIds { get; set; }
        public List<UpdateUnitResult> UpdateUnitResults { get; set; }
    }
}
