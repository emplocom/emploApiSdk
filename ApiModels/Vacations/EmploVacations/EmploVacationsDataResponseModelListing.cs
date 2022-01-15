using EmploApiSDK.ApiModels.Vacations.IntegratedVacationWebhooks.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.EmploVacations
{
    public class EmploVacationsDataResponseModelListing
    {
        public List<EmploVacationsDataResponseModel> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class EmploVacationsDataResponseModel
    {
        public int VacationId { get; set; }
        public string EmployeeId { get; set; }
        public string IntegratedVacationTypeId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public string Description { get; set; }
        public VacationStatusEnum Status { get; set; }
        public Decimal Duration { get; set; }
        public Decimal? AbsenceHours { get; set; }
    }
}
