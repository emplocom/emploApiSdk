using System;
using System.Collections.Generic;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationDelete
{
    public class DeleteIntegratedVactionsRequestModel
    {
        public List<string> ExternalVacationIds { get; set; }
        public int ExternalSystemId { get; set; }
    }
}
