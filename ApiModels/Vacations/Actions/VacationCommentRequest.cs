using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.ApiModels.Vacations.Actions
{
    public class VacationCommentRequest
    {
        /// <summary>
        /// Content of the comment
        /// </summary>
        public string Content { get; set; }

        public List<int> GalleryImageIds { get; set; }

        public List<int> AttachmentFileIds { get; set; }
    }

}
