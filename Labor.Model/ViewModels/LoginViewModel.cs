﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class LoginViewModel
    {
        /// <summary>
        /// 登录帐号
        /// </summary>
        [Required]
        public string DomainAccount { get; set; }

    }
}
