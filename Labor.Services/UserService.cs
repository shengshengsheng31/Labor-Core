using Labor.Common;
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


    }
}
