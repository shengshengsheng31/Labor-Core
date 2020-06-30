using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.Models
{
    public class DefaultLaborViewModel
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public string Option { get; set; }
        /// <summary>
        /// 劳保品
        /// </summary>
        public string Goods { get; set; }
        /// <summary>
        /// 劳保Id
        /// </summary>
        public Guid LaborId { get; set; }
    }
}
