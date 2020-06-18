using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
