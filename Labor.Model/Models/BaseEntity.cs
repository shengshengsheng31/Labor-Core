using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.Models
{
    /// <summary>
    /// model基类
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
