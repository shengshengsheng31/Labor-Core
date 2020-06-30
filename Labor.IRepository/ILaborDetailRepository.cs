using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.IRepository
{
    public interface ILaborDetailRepository:IBaseRepository<LaborDetail>
    {
        /// <summary>
        /// 获取劳保通过LaborId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<LaborDetailListViewModel> GetAllByHead(GetLaborDetailViewModel model);

        /// <summary>
        /// 设置默认劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task SetDefaultLabor(DefaultLaborViewModel model);
    }
}
