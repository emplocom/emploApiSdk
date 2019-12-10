using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Common
{
    public enum ImportStatuses
    {
        Ok,
        MissingData,
        InvalidData,
        NotImplemented,
        ObjectAlreadyExists,
        Error,
        Skipped
    }
}
