using System;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Razor.Language;
using NPOI.SS.Formula.Functions;

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
        public async Task<IActionResult> LoginAsync(string callback,string UserName,string Password)
        {
            string domainAccount = HttpContext.User.Identity.Name.Split('\\')[1];
            //手动域账号登录
            if (UserName != null)
            {
                try
                {
                    using (LoginHelper wi = new LoginHelper(UserName, HttpContext.User.Identity.Name.Split('\\')[0], Password))
                    {
                        domainAccount = UserName;
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest($"域账号错误{ex.Message}");
                }
            }

            User user = await _userService.LoginAsync(new LoginViewModel { DomainAccount=domainAccount});

            if (user == null)
            {
                return BadRequest($"用户{domainAccount}不存在");
            }
            TokenModelJwt tokenModel = new TokenModelJwt { UserId=user.Id,Level=user.Level.ToString(),Account=user.UserName,DeptId=user.DepartmentId };
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
        public async Task<IActionResult> GetAllUser([FromQuery]GetUserViewModel model)
        {
            PageInfoHelper<User> result = await _userService.GetAllUser(model);
            Response.Headers.Add("Pagination-X", JsonSerializer.Serialize(result.TotalCount));
            return Ok(result);
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

        /// <summary>
        /// 根据Id删除用户
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {
            await _userService.RemoveAsync(Id);
            return Ok();
        }

        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(ChangeRole))]
        public async Task<IActionResult> ChangeRole([FromBody] UpdateRoleViewModel model)
        {
            await _userService.UpdateRole(model);
            return Ok();
        }

        /// <summary>
        /// 通过工号获取用户
        /// </summary>
        /// <param name="EmpNo"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetUserByNum))]
        public async Task<IActionResult> GetUserByNum(int EmpNo)
        {
            return Ok(await _userService.GetOneUserByNum(EmpNo));
            
        }
    }
}