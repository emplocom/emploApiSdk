using System.Collections.Generic;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationBalances
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
