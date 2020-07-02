using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class RegisterViewModel
    {
        /// <summary>
        /// 域帐号
        /// </summary>
        public string DomainAccount { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Required]
        public int EmpNo { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Required]
        public Level Level { get; set; } = Level.user;
    }
}
