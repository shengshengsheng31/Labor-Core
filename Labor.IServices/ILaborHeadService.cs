using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using System.Threading.Tasks;

namespace Labor.IServices
{
    public interface ILaborHeadService:IBaseService<LaborHead>
    {
        /// <summary>
        /// 新建劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> CreateLaborHeadAsync(EditLaborHeadViewModel model);

        /// <summary>
        /// 根据标题获取一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LaborHead> GetOneLaborHead(GetLaborHeadViewModel model);

        /// <summary>
        /// 获取最近一期的劳保
        /// </summary>
        /// <returns></returns>
        Task<LaborHead> GetLaborLatest();

        /// <summary>
        /// 修改一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateLaborHeadAsync(EditLaborHeadViewModel model);

        /// <summary>
        /// 分页获取所有数据，返回数据与页码信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PageInfoHelper<LaborHead>> GetAllLaborAsync(PageViewModel model);
    }
}
