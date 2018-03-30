using System.Collections.Generic;

namespace EmploApiSDK.Models
{
    public class FinishImportResponseModel
    {
        public ImportStatusCode ImportStatusCode { get; set; }
        public List<int> BlockedUserIds { get; set; }
        public List<UpdateUnitResult> UpdateUnitResults { get; set; }
    }
}
