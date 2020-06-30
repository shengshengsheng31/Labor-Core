using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class GetLaborDetailViewModel:PageViewModel
    {
        /// <summary>
        /// 劳保期数Id
        /// </summary>
        public Guid LaborId { get; set; }

        /// <summary>
        /// 劳保标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; }
    }
}
