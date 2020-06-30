using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Model.ViewModels
{
    public class LaborDetailListViewModel
    {
        public string Account { get; set; }
        public string Option { get; set; }
        public string Goods { get; set; }
        public string Department { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
