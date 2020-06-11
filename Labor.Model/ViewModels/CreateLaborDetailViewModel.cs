using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class CreateLaborDetailViewModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 某期劳保Id
        /// </summary>
        public Guid LaborId { get; set; }

        /// <summary>
        /// 选项ABC
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// 劳保品
        /// </summary>
        public string Goods { get; set; }
    }
}
