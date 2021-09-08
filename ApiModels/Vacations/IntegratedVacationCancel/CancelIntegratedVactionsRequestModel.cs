using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationCancel
{
    public class CancelIntegratedVactionsRequestModel
    {
        public CancelIntegratedVactionsRequestModel()
        {
        }

        public List<int> VacationIds { get; set; }
        public string Comment { get; set; }
    }
}
