﻿using System;
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
        [HttpGet(nameof(GetLaborDetailByHeadPage))]
        public async Task<IActionResult> GetLaborDetailByHeadPage([FromQuery]GetLaborDetailViewModel model)
        {
            PageInfoHelper<LaborDetailListViewModel> result = await _laborHeadService.GetAllByHeadPage(model);
            Response.Headers.Add("Pagination-X", JsonSerializer.Serialize(result.TotalCount));
            return Ok(result);
        }

        /// <summary>
        /// 通过用户Id和劳保Id获取用户的选项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetUserLaborChoice))]
        public async Task<IActionResult> GetUserLaborChoice([FromQuery]OneLaborDetailViewModel model)
        {
            return Ok(await _laborHeadService.GetLaborDetailByUserAndLaborAsync(model));
        }

        /// <summary>
        /// 导出劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(ExportLabor))]
        public IActionResult ExportLabor([FromQuery]GetLaborDetailViewModel model)
        {
            return File(_laborHeadService.ExlExport(model), "application/ms-excel", $"{model.Title}-{DateTime.Today}.xlsx");
        }

        /// <summary>
        /// 设置默认劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost(nameof(SetDefaultLabor))]
        public async Task<IActionResult> SetDefaultLabor(DefaultLaborViewModel model)
        {
            await _laborHeadService.SetDefaultLabor(model);
            return Ok();
        }

        /// <summary>
        /// 获取选项的比例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet(nameof(GetOptionRate))]
        public async Task<IActionResult> GetOptionRate([FromQuery]GetOptionRateViewModel model)
        {
            return Ok(await _laborHeadService.GetOptionRate(model));
        }
    }
}