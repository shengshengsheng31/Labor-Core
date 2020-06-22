using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly Guid _userId;
        public DepartmentController(IDepartmentService departmentService, IHttpContextAccessor httpContext)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
            IHttpContextAccessor accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateDept))]
        public async Task<IActionResult> CreateDept(CreateDeptViewModel model)
        {
            if(await _departmentService.CreateDept(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("创建失败");
            }
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(UpdateDept))]
        public async Task<IActionResult> UpdateDept(UpdateDeptViewModel model)
        {
            await _departmentService.UpdateDept(model);
            return Ok();
        }

        /// <summary>
        /// 删除部门，将级联删除所关联的人员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid Id)
        {
            await _departmentService.RemoveAsync(Id);
            return Ok();
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
         [HttpGet(nameof(GetAllDepartment))]
        public IActionResult GetAllDepartment()
        {
            return Ok(_departmentService.GetAll());
        }

    }
}