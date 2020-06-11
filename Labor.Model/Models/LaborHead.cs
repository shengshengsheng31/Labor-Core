using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Labor.Model.Models
{
    public class LaborHead:BaseEntity
    {
        /// <summary>
        /// 名称，默认为当前月+自定义名称
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
