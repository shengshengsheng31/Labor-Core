using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Labor.Model.Models
{
    public class LaborDetail:BaseEntity
    {
        /// <summary>
        /// 外键用户
        /// </summary>
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// 外键劳保品
        /// </summary>
        public Guid LaborId { get; set; }
        public LaborHead Labor { get; set; }

    }
}
