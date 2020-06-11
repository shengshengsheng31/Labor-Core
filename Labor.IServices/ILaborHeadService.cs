using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        Task<bool> CreateLaborHeadAsync(CreateLaborHeadViewModel model);

        /// <summary>
        /// 根据标题获取一期劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LaborHead> GetLaborHeadByTitle(GetLaborHeadViewModel model);

    }
}
