using System;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationValidation
{
    public class IntegratedVacationValidationExternalRequest
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

        public string ExternalVacationRequestId { get; set; }
    }
}
