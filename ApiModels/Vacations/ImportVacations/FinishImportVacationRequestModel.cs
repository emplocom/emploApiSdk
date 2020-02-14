using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.ImportVacations
{
    public class FinishImportVacationRequestModel
    {
        public FinishImportVacationRequestModel(string importId)
        {
            ImportId = importId;
        }

        /// <summary>
        /// Id of the import which is being finished
        /// </summary>
        public string ImportId { get; set; }
    }
}
