using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class LaborDetailService:BaseService<LaborDetail>,ILaborDetailService
    {
        private readonly ILaborDetailRepository _laborDetailRepository;
        public LaborDetailService(ILaborDetailRepository laborDetailRepository)
        {
            _laborDetailRepository = laborDetailRepository;
            BaseRepository = laborDetailRepository;
        }

        /// <summary>
        /// 创建一个人的劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateLaborDetailAsync(CreateLaborDetailViewModel model)
        {
            if (await _laborDetailRepository.GetAll().AnyAsync(m =>m.UserId==model.UserId && m.LaborId==model.LaborId))
            {
                return false;
            }
            else
            {
                await _laborDetailRepository.CreateAsync(new LaborDetail
                {
                    UserId = model.UserId,
                    LaborId = model.LaborId,
                    Option = model.Option,
                    Goods = model.Goods
                });
                return true;
            }
        }

        /// <summary>
        /// 获取劳保通过LaborId
        /// </summary>
        /// <returns></returns>
        public IQueryable GetAllByHead(GetLaborDetailViewModel model)
        {
            return  _laborDetailRepository.GetAll().Where(m => m.LaborId == model.LaborId).Include(m=>m.Labor);
        }

    }
}
