using Labor.Common;
using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Helpers;
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
            if (await _userRepository.GetAll().AnyAsync(m => m.DomainAccount == model.DomainAccount || m.EmpNo == model.EmpNo))
            {
                return false;
            }
            else
            {
                await _userRepository.CreateAsync(new User
                {
                    DomainAccount = model.DomainAccount,
                    UserName = model.UserName,
                    DepartmentId = model.DepartmentId,
                    Level = model.Level,
                    EmpNo = model.EmpNo,
                });
                return true;
            }
        }

        /// <summary>
        /// 默认获取所有用户,如果有部门名将查询对应的部门人员
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<PageInfoHelper<User>>  GetAllUser(GetUserViewModel model)
        {
            IQueryable<User> result = _userRepository.GetAllByOrder().OrderBy(m=>m.DepartmentId).ThenBy(m=>m.EmpNo).Include(m=>m.Department);
            if (model.DeptId != Guid.Empty)
            {
                result = result.Where(m => m.DepartmentId == model.DeptId);
            }
            return await PageInfoHelper<User>.CreatePageMsgAsync(result, model.PageNumber, model.PageSize);
        }

        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateRole(UpdateRoleViewModel model)
        {
            await _userRepository.EditAsync(new User
            {
                Id = model.Id,
                Level = model.Level,
                DepartmentId = model.DepartmentId,
                DomainAccount =model.DomainAccount,
                EmpNo = model.EmpNo,
                UserName = model.UserName
            });
        }

        /// <summary>
        /// 通过工号查询
        /// </summary>
        /// <param name="empNum"></param>
        /// <returns></returns>
        public async Task<User> GetOneUserByNum(int empNum)
        {
            User user = await _userRepository.GetAll().Where(m => m.EmpNo == empNum).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateUser(UpdateUserViewModel model)
        {
            User user = await _userRepository.GetAll().FirstAsync(m => m.Id == model.Id);
            user.UserName = model.UserName;
            user.DepartmentId = model.DepartmentId;
            user.DomainAccount = model.DomainAccount;
            user.EmpNo = model.EmpNo;
            user.Level = model.Level;
            user.UpdateTime = DateTime.Now;
            await _userRepository.EditAsync(user);
        }

        /// <summary>
        /// 通过条件获取用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<User> GetUserByQuery(GetUserViewModel model)
        {
            IQueryable<User> result = _userRepository.GetAll();
            string type = model.QueryType;
            switch (type)
            {
                case "UserName":
                    result = result.Where(m => m.UserName == model.QueryString);
                    break;
                case "EmpNo":
                    result = result.Where(m => m.EmpNo ==Convert.ToInt32(model.QueryString));
                    break;
                case "DomainAccount":
                    result = result.Where(m => m.DomainAccount == model.QueryString);
                    break;
                default:
                    break;
            }
            return result;

        }
    }
}
