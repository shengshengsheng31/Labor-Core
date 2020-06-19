using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class GetLaborDetailViewModel
    {
        /// <summary>
        /// 劳保期数Id
        /// </summary>
        public Guid LaborId { get; set; }

        /// <summary>
        /// 劳保标题
        /// </summary>
        public string Title { get; set; }
    }
}
