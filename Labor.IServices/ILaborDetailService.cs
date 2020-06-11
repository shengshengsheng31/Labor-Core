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
    }
}
