using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Models
{
    public class ImportIntegratedVacationsBalanceDataRequestModel
    {
        public List<IntegratedVacationsBalanceDto> BalanceList;
    }

    public class IntegratedVacationsBalanceDto
    {
        public string ExternalEmployeeId;
        public string ExternalVacationTypeId;
        public decimal AvailableDays;
        public decimal AvailableHours;
        public decimal OutstandingDays;
        public decimal OutstandingHours;
        public decimal OnDemandDays;
    }
}
