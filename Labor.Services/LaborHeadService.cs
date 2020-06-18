using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
        public async Task<bool> CreateLaborHeadAsync(EditLaborHeadViewModel model)
        {
            if (await _laborHeadRepository.GetAll().AnyAsync(m => m.Title == model.Title))
            {
                return false;
            }
            else
            {
                await _laborHeadRepository.CreateAsync(new LaborHead()
                {
                    Title = model.Title,
                    Options = model.Options,
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
        public async Task<LaborHead> GetOneLaborHead(GetLaborHeadViewModel model)
        {
            LaborHead result;
            if (model.Id != Guid.Empty)
            {
                result = await _laborHeadRepository.GetOneByIdAsync(model.Id);
            }
            else
            {
                result = await _laborHeadRepository.GetAll().FirstOrDefaultAsync(m => m.Title == model.Title);
            }
            return result;
        }

        /// <summary>
        /// 获取最近一期的劳保
        /// </summary>
        /// <returns></returns>
        public Task<LaborHead> GetLaborLatest()
        {
            return _laborHeadRepository.GetAllByOrder().FirstAsync();
        }

        /// <summary>
        /// 修改一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateLaborHeadAsync(EditLaborHeadViewModel model)
        {

            await _laborHeadRepository.EditAsync(new LaborHead()
            {
                Id = model.Id,
                Goods = model.Goods,
                Options = model.Options,
                UpdateTime = DateTime.Now,
                Title = model.Title
            });
        }

        /// <summary>
        /// 分页获取所有数据，返回数据与页码信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PageInfoHelper<LaborHead>> GetAllLaborAsync(PageViewModel model)
        {
            IQueryable<LaborHead> laborHeads = _laborHeadRepository.GetAllByOrder();
            return await PageInfoHelper<LaborHead>.CreatePageMsgAsync(laborHeads, model.PageNumber, model.PageSize);
        }
    }
}
