using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Cache;

namespace Labor.Model.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User:BaseEntity
    {
        /// <summary>
        /// 域账号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DomainAccount { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [Required]
        public Level Level { get; set; } = Level.user;

        /// <summary>
        /// 部门
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }

    public enum Level
    {
        user=0,
        deptManager = 1,
        admin=2
    }

}
