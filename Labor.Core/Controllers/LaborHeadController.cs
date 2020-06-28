using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborHeadController : ControllerBase
    {
        private readonly ILaborHeadService _laborHeadService;
        private readonly Guid _userId;

        public LaborHeadController(ILaborHeadService laborHeadService, IHttpContextAccessor httpContext)
        {
            _laborHeadService = laborHeadService ?? throw new ArgumentNullException(nameof(laborHeadService));
            IHttpContextAccessor accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }

        /// <summary>
        /// 创建一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateLaborHead))]
        public async Task<IActionResult> CreateLaborHead(EditLaborHeadViewModel model)
        {
            if (await _laborHeadService.CreateLaborHeadAsync(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("同期已存在相同选项的劳保");
            }
        }

        /// <summary>
        /// 获取一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetOneLaborHead))]
        public async Task<IActionResult> GetOneLaborHead([FromQuery]GetLaborHeadViewModel model)
        {
            return Ok(await _laborHeadService.GetOneLaborHead(model));
        }

        /// <summary>
        /// 根据时间分页获取所有的劳保,返回数据及总数
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetAllLaborHead))]
        public async Task<IActionResult> GetAllLaborHead([FromQuery]PageViewModel model)
        {
            PageInfoHelper<LaborHead> result = await _laborHeadService.GetAllLaborAsync(model);
            Response.Headers.Add("Pagination-X", JsonSerializer.Serialize(result.TotalCount));
            return Ok(result);
        }

        /// <summary>
        /// 获取最近的一次劳保
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetLaborLatest))]
        public async Task<IActionResult> GetLaborLatest()
        {
            return Ok(await _laborHeadService.GetLaborLatest());
        }

        /// <summary>
        /// 修改一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(UpdateLaborHead))]
        public async Task<IActionResult> UpdateLaborHead(EditLaborHeadViewModel model)
        {
            await _laborHeadService.UpdateLaborHeadAsync(model);
            return Ok();
        }

        /// <summary>
        /// 删除劳保
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Guid Id)
        {
            await _laborHeadService.RemoveAsync(Id);
            return Ok();
        }
    }
}