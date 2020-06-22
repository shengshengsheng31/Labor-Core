using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class DepartmentService:BaseService<Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
            BaseRepository = departmentRepository;
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateDept(CreateDeptViewModel model)
        {

            //暂时不允许同名的部门
            if (await _departmentRepository.GetAll().AnyAsync(m => m.DeptName == model.DeptName))
            {
                return false;
            }
            else
            {
                await _departmentRepository.CreateAsync(new Department
                {
                    DeptName = model.DeptName,
                });
                return true;
            }
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateDept(UpdateDeptViewModel model)
        {
            await _departmentRepository.EditAsync(new Department
            {
                Id = model.Id,
                DeptName = model.DeptName,
                UpdateTime = DateTime.Now
            });
        }
    }
}
