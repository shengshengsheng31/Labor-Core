using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
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
        public async Task<IActionResult> CreateLaborHead(CreateLaborHeadViewModel model)
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
            return Ok(await _laborHeadService.GetLaborHeadByTitle(model));
        }

        /// <summary>
        /// 获取所有的劳保
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetAllLaborHead))]
        public IActionResult GetAllLaborHead()
        {
            return Ok( _laborHeadService.GetAll());
        }
    }
}