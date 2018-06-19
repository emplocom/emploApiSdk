using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.IntegratedVacationValidation
{
    public class VacationValidationResponseModel
    {
        /// <summary>
        /// Vacation request validation result
        /// </summary>
        public bool RequestIsValid;

        /// <summary>
        /// Message detailing validation results
        /// </summary>
        public string Message;

        /// <summary>
        /// Optional additional information sent back to emplo along with validation result
        /// </summary>
        public List<string> AdditionalMessagesCollection = new List<string>();
    }
}
