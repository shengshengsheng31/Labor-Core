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
        IQueryable GetAllByHead(GetLaborDetailViewModel model);

        /// <summary>
        /// 通过用户Id和LaborHead Id 来获取一条已选择的劳保
        /// </summary>
        /// <returns></returns>
        Task<LaborDetail> GetLaborDetailByUserAndLaborAsync(OneLaborDetailViewModel model);
    }
}
