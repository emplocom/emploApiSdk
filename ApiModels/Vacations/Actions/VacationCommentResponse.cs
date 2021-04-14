using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.Actions
{
    public class VacationCommentResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// The content of comment
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// A date of creation
        /// </summary>
        public string Date { get; set; }

    }
}
