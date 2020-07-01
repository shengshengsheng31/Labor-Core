using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class GetOptionRateViewModel
    {
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<string> Options { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; }

        /// <summary>
        /// 劳保期数
        /// </summary>
        public Guid LaborId { get; set; }

    }
}
