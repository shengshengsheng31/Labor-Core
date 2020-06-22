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
    }
}
