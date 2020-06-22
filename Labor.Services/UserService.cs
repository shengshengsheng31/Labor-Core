using Labor.Common;
using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class UserService:BaseService<User>,IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            BaseRepository = userRepository;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<User> LoginAsync(LoginViewModel model)
        {
            return await _userRepository.GetAll().FirstOrDefaultAsync(m => m.DomainAccount == model.DomainAccount );
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            //存在域账号
            if (await _userRepository.GetAll().AnyAsync(m => m.DomainAccount == model.DomainAccount))
            {
                return false;
            }
            else
            {
                await _userRepository.CreateAsync(new User
                {
                    DomainAccount = model.DomainAccount,
                    UserName = model.Name,
                    DepartmentId = model.DepartmentId,
                    Level = model.Level,
                });
                return true;
            }
        }

        /// <summary>
        /// 默认获取所有用户,如果有部门名将查询对应的部门人员
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public  IQueryable GetAllUser(GetUserViewModel model)
        {
            IQueryable<User> result = _userRepository.GetAllByPageOrder(model.PageSize, model.PageNumber);
            if (model.DeptId != Guid.Empty)
            {
                result = result.Where(m => m.DepartmentId == model.DeptId).Include(m=>m.Department);
            }
            return result;
        }


    }
}
