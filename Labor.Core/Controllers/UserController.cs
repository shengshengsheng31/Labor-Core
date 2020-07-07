using System;
using System.IO;
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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly Guid _userId;
        public UserController(IUserService userService, IDepartmentService departmentService, IHttpContextAccessor httpContext)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _departmentService = departmentService?? throw new ArgumentNullException(nameof(departmentService));
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
            //启用超级用户，不经过windows域验证
            if (UserName == "Ferrotec" && Password == "Ferrotec")
            {
                //生成JWT
                TokenModelJwt tokenModel = new TokenModelJwt { UserId = Guid.Empty, Level = Level.admin.ToString(), Account = UserName, DeptId = Guid.Empty };
                string token = JwtHelper.JwtEncrypt(tokenModel);
                token = JsonSerializer.Serialize(token);
                return Ok($"{callback}({token})");
            }
            else
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
                    catch (Exception ex)
                    {
                        return BadRequest($"域账号错误{ex.Message}");
                    }
                }

                User user = await _userService.LoginAsync(new LoginViewModel { DomainAccount = domainAccount });

                if (user == null)
                {
                    return BadRequest($"用户{domainAccount}不存在");
                }


                //生成JWT
                TokenModelJwt tokenModel = new TokenModelJwt { UserId = user.Id, Level = user.Level.ToString(), Account = user.UserName, DeptId = user.DepartmentId };
                string token = JwtHelper.JwtEncrypt(tokenModel);
                token = JsonSerializer.Serialize(token);
                return Ok($"{callback}({token})");
            }
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

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(UpdateUser))]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            await _userService.UpdateUser(model);
            return Ok();
        }

        /// <summary>
        /// 通过条件获取用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetUserByQuery))]
        public  IActionResult GetUserByQuery([FromQuery]GetUserViewModel model)
        {
            return Ok(_userService.GetUserByQuery(model));
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost(nameof(ImportExcel))]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        {
            int countOk = 0;
            int countSkip = 0;
            try
            { 
                Stream stream = file.OpenReadStream();
                stream.Seek(0, SeekOrigin.Begin);
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);
                int countRow = sheet.LastRowNum + 1;
                for (int i = 1; i < countRow; i++)
                {
                    IRow currentRow = sheet.GetRow(i);
                    string userName = Convert.ToString(currentRow.GetCell(0)).Trim();
                    bool intFlag = int.TryParse(Convert.ToString(currentRow.GetCell(1)), out int empNo);
                    string domainAccount = Convert.ToString(currentRow.GetCell(2)).Trim();
                    string deptName = Convert.ToString(currentRow.GetCell(3)).Trim();
                    //验证数据有效性
                    if (userName == "" || empNo==0 || deptName == "")
                    {
                        countSkip++;
                        continue;
                    }
                    //已存在
                    if (await _userService.GetAll().AnyAsync(m => m.EmpNo == empNo))
                    {
                        countSkip++;
                        continue;
                    }
                    Department dept = await _departmentService.GetAll().FirstAsync(m => m.DeptName == Convert.ToString(currentRow.GetCell(3)));
                    await _userService.CreateAsync(new User
                    {
                        UserName = userName,
                        EmpNo = empNo,
                        DomainAccount = domainAccount,
                        DepartmentId = dept.Id
                    }, false);
                    countOk++;

                }
                await _userService.SaveAsync();
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new {  countOk,  countSkip });
        }
    }
}