using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Labor.Model.Models
{
    public class RoleToMenu:BaseEntity
    {
        /// <summary>
        /// 外键菜单
        /// </summary>
        public Guid MenuId { get; set; }

        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; }

        /// <summary>
        /// 外键角色
        /// </summary>
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
