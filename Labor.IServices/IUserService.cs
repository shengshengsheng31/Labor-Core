﻿using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.IServices
{
    public interface IUserService:IBaseService<User>
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<User> LoginAsync(LoginViewModel model);

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
       IQueryable GetAllUser(GetUserViewModel model);
    }
}
