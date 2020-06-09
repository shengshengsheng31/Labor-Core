using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Cache;

namespace Labor.Model.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User:BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [Required]
        public Level Level { get; set; } = Level.user;
    }

    public enum Level
    {
        user=0,
        admin = 1
    }

}
