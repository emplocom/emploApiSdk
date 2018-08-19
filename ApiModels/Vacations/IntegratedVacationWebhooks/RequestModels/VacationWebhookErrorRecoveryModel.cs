using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationWebhooks.RequestModels
{
    public class VacationWebhookErrorRecoveryModel
    {
        /// <summary>
        /// External id of request which needs to be deleted
        /// </summary>
        public string ExternalVacationId { get; set; }

        /// <summary>
        /// Vacation type identifier used by external system
        /// </summary>
        public string ExternalVacationTypeId { get; set; }

        /// <summary>
        /// External id of request which needs to be deleted
        /// </summary>
        public string ExternalEmployeeId { get; set; }

        /// <summary>
        /// Is there an externally managed vacation days balance for this type, 
        /// that will be synchronized and displayed in Emplo?
        /// </summary>
        public bool HasManagedVacationDaysBalance { get; set; }

        /// <summary>
        /// Time of operation occurrence in emplo
        /// </summary>
        public DateTime OperationTime { get; set; }
    }
}
