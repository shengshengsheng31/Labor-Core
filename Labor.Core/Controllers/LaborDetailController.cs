using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labor.Common;
using Labor.IServices;
using Labor.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Labor.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborDetailController : ControllerBase
    {
        private readonly ILaborDetailService _laborHeadService;
        private readonly Guid _userId;

        public LaborDetailController(ILaborDetailService laborHeadService, IHttpContextAccessor httpContext)
        {
            _laborHeadService = laborHeadService ?? throw new ArgumentNullException(nameof(httpContext));
            IHttpContextAccessor accessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _userId = JwtHelper.JwtDecrypt(accessor.HttpContext.Request.Headers["Authorization"]).UserId;
        }


        /// <summary>
        /// 用户选择劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(CreateLaborDetail))]
        public async Task<IActionResult> CreateLaborDetail(CreateLaborDetailViewModel model)
        {
            if(await  _laborHeadService.CreateLaborDetailAsync(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("用户已选择过劳保");
            }
        }

        /// <summary>
        /// 根据劳保期数显示所有人选择的劳保
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(GetLaborDetailByHead))]
        public  IActionResult GetLaborDetailByHead([FromQuery]GetLaborDetailViewModel model)
        {
            return Ok(_laborHeadService.GetAllByHead(model));
        }
    }
}