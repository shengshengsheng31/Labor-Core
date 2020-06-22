using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.Models
{
    public class Department:BaseEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        public string DeptName { get; set; }
    }
}
