using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class OneLaborDetailViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 劳保Id
        /// </summary>
        public Guid LaborId { get; set; }
    }
}
