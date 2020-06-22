using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labor.IServices
{
    public interface IDepartmentService:IBaseService<Department>
    {
        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> CreateDept(CreateDeptViewModel model);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateDept(UpdateDeptViewModel model);
    }
}
