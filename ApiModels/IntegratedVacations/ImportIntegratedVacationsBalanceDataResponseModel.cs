using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Models
{
    public enum ImportVacationDataStatusCode
    {
        Ok,
        Error
    }

    public class ImportIntegratedVacationsBalanceDataResponseModel
    {
        public ImportVacationDataStatusCode OperationStatus;
        public string ErrorMessage = string.Empty;
    }
}
