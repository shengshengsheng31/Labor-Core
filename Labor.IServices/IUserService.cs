﻿using Labor.Model.Helpers;
using Labor.Model.Models;
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
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PageInfoHelper<User>> GetAllUser(GetUserViewModel model);

        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateRole(UpdateRoleViewModel model);

        /// <summary>
        /// 通过工号查询
        /// </summary>
        /// <param name="empNum"></param>
        /// <returns></returns>
        Task<User> GetOneUserByNum(int empNum);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateUser(UpdateUserViewModel model);

        /// <summary>
        /// 条件查询用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<User> GetUserByQuery(GetUserViewModel model);
    }
}
