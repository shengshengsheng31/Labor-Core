using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class PageViewModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Required]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        [Required]
        public int PageSize { get; set; } = 20;
    }
}
