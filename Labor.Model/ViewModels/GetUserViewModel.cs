using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class GetUserViewModel : PageViewModel
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DeptId { get; set; } = Guid.Empty;

        /// <summary>
        /// 类别
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        /// 具体字段
        /// </summary>
        public string QueryString { get; set; }

    }
}
