using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.ImportVacations
{
    public class ImportVacationRequestModel
    {
        public ImportVacationRequestModel()
        {
            Rows = new List<VacationDataRow>();
        }

        public string ImportId { get; set; }

        /// <summary>
        /// Data to import
        /// </summary>
        public List<VacationDataRow> Rows { get; set; }

    }
}
