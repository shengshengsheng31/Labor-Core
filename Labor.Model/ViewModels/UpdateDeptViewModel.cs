using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class UpdateDeptViewModel
    {
        /// <summary>
        /// 修改后的部门名
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 需要修改的部门Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
