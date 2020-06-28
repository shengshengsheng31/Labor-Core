using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class UpdateRoleViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 域账号
        /// </summary>
        public string DomainAccount { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public int EmpNo { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public Level Level { get; set; } = Level.user;

        /// <summary>
        /// 部门
        /// </summary>
        public Guid DepartmentId { get; set; }

    }
}
