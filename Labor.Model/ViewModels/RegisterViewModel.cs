using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class RegisterViewModel
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(50,MinimumLength =2)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
