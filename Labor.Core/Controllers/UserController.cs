using System;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            User user = await _userService.LoginAsync(model);
            if (user == null)
            {
                return BadRequest("用户不存在");
            }
            TokenModelJwt tokenModel = new TokenModelJwt { UserId=user.Id,Level=user.Level.ToString() };
            string token = JwtHelper.JwtEncrypt(tokenModel);
            return Ok(token);
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
        /// 获取所有用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetAllUser))]
        public IActionResult GetAllUser([FromQuery]PageViewModel model)
        {
            return Ok(_userService.GetAllByPageOrder(model.PageSize, model.PageNumber));
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

    }
}