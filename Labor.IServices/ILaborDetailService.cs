using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.IServices
{
    public interface ILaborDetailService : IBaseService<LaborDetail>
    {
        /// <summary>
        /// 创建一个人的劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> CreateLaborDetailAsync(CreateLaborDetailViewModel model);

        /// <summary>
        /// 获取劳保通过LaborId
        /// </summary>
        /// <returns></returns>
        IQueryable<LaborDetailListViewModel> GetAllByHead(GetLaborDetailViewModel model);

        /// <summary>
        /// 通过用户Id和LaborHead Id 来获取一条已选择的劳保
        /// </summary>
        /// <returns></returns>
        Task<LaborDetail> GetLaborDetailByUserAndLaborAsync(OneLaborDetailViewModel model);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        byte[] ExlExport(GetLaborDetailViewModel model);

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PageInfoHelper<LaborDetailListViewModel>> GetAllByHeadPage(GetLaborDetailViewModel model);

        /// <summary>
        /// 设置默认劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task SetDefaultLabor(DefaultLaborViewModel model);
    }
}
