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
    public class LaborHeadService:BaseService<LaborHead>,ILaborHeadService
    {
        private readonly ILaborHeadRepository _laborHeadRepository;
        public LaborHeadService(ILaborHeadRepository laborHeadRepository)
        {
            _laborHeadRepository = laborHeadRepository;
            BaseRepository = laborHeadRepository;
        }

        /// <summary>
        /// 新建劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateLaborHeadAsync(CreateLaborHeadViewModel model)
        {
            if (await _laborHeadRepository.GetAll().AnyAsync(m => m.Title == model.Title && m.Option == model.Option))
            {
                return false;
            }
            else
            {
                await _laborHeadRepository.CreateAsync(new LaborHead()
                {
                    Title = model.Title,
                    Option = model.Option,
                    Goods = model.Goods
                });
                return true;
            }
        }

        /// <summary>
        /// 根据标题获取一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<LaborHead>> GetLaborHeadByTitle(GetLaborHeadViewModel model)
        {
            return await _laborHeadRepository.GetAll().Where(m => m.Title == model.Title).ToListAsync();
            //List<LaborHead> labor = await _laborHeadRepository.GetAll().Where(m => m.Title == model.Title).ToListAsync();



        }
    }
}
