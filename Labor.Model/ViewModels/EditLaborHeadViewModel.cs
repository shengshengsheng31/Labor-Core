﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class EditLaborHeadViewModel
    {
        /// <summary>
        /// 传入Id表示修改
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 选项ABC
        /// </summary>
        [Required]
        public string Options { get; set; }

        /// <summary>
        /// 劳保品
        /// </summary>
        [Required]
        public string Goods { get; set; }
    }
}
