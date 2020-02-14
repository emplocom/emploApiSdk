using System.Collections.Generic;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationDelete
{
    public class DeleteIntegratedVactionsResponseModel
    {
        public List<DeleteIntegratedVactionsResponseRow> Operations { get; set; }

        public DeleteIntegratedVactionsResponseModel()
        {
            Operations = new List<DeleteIntegratedVactionsResponseRow>();
        }

    }

    public class DeleteIntegratedVactionsResponseRow
    {
        public string ExternalVacationId { get; set; }
        public string Message { get; set; }
        public DeleteIntegrationVacationStatus Status { get; set; }
    }

    public enum DeleteIntegrationVacationStatus
    {
        Success,
        Failed
    }
}
