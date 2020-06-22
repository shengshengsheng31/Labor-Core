﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.Language;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Guid _userId;
        public UserController(IUserService userService, IHttpContextAccessor httpContext)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            IHttpContextAccessor accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            if (accessor.HttpContext.Request.Path.Value != "/api/User/Login")
            {
                _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Login")]
        public async Task<IActionResult> LoginAsync(string callback)
        {
            string domainAccount = HttpContext.User.Identity.Name.Split('\\')[1];
            User user = await _userService.LoginAsync(new LoginViewModel { DomainAccount=domainAccount});

            if (user == null)
            {
                return BadRequest($"用户{domainAccount}不存在");
            }
            TokenModelJwt tokenModel = new TokenModelJwt { UserId=user.Id,Level=user.Level.ToString(),Account=user.UserName };
            string token =JwtHelper.JwtEncrypt(tokenModel);
            token = JsonSerializer.Serialize(token);
            return Ok($"{callback}({token})");
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (await _userService.RegisterAsync(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("用户已存在，注册失败");
            }
        }

        /// <summary>
        /// 根据部门获取所有用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetAllUser))]
        public IActionResult GetAllUser([FromQuery]GetUserViewModel model)
        {
            return Ok(_userService.GetAllUser(model));
        }

        /// <summary>
        /// 验证Jwt
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost(nameof(TestJwt))]
        public IActionResult TestJwt([FromQuery] string token)
        {
            
            return Ok(JwtHelper.JwtDecrypt(token));
        }

        /// <summary>
        /// windows验证用户域账号
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(VerifyUser))]
        public IActionResult VerifyUser(string callback)
        {
            string doaminAccount = HttpContext.User.Identity.Name;

            //string domainAccount = JsonSerializer.Serialize(HttpContext.User.Identity.Name);
            
            
            return Ok($"{callback}({123})");
        }
    }
}