using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationCancel
{
    public class CancelIntegratedVactionsResponseModel
    {
        public string Message { get; set; }
        public CancelIntegrationVacationStatusEnum Status { get; set; }
        public Dictionary<int, CancelIntegrationVacationStatusEnum> VacationsCancellationStatuses { get; set; } = new Dictionary<int, CancelIntegrationVacationStatusEnum>();
    }
}
