using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploApiSDK.ApiModels.Vacations.IntegratedVacationWebhooks.RequestModels;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationValidation
{
    public class VacationValidationRequestModel
    {
        /// <summary>
        /// Employee identifier used by integrated system
        /// </summary>
        public string ExternalEmployeeId { get; set; }

        /// <summary>
        /// Vacation type identifier used by external system
        /// </summary>
        public string ExternalVacationTypeId { get; set; }

        /// <summary>
        /// Vacation request status depend on acceptance flow configured for this request type 
        /// and can contain following statuses:
        /// ForApproval = 1, when request needs acceptance
        /// Accepted = 2, when request was automatically accepted
        /// Executed = 4, when request was automatically accepted and executed
        /// </summary>
        public VacationStatusEnum Status { get; set; }

        /// <summary>
        /// Vacation start date
        /// </summary>
        public DateTime Since { get; set; }

        /// <summary>
        /// Vacation end date
        /// </summary>
        public DateTime Until { get; set; }

        /// <summary>
        /// Number of days used by this vacation request (after subtracting all free days from 
        /// assigned free days calendar id present)
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        /// How many hours requested if it not a full day leave
        /// </summary>
        public decimal? AbsenceHours { get; set; }

        /// <summary>
        /// Is this an On Demand request
        /// </summary>
        public bool IsOnDemand { get; set; }
    }
}
