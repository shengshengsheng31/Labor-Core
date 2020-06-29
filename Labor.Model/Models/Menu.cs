using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.Models
{
    public class Menu:BaseEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 菜单序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }


    }
}
