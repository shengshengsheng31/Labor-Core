using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Repository
{
    public class LaborDetailRepository : BaseRepository<LaborDetail>, ILaborDetailRepository
    {
        private readonly LaborContext _context;
        public LaborDetailRepository(LaborContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取劳保通过LaborId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<LaborDetailListViewModel> GetAllByHead(GetLaborDetailViewModel model)
        {
            IQueryable<LaborDetailListViewModel> result = from user in _context.User
                                                          join laborDetail in _context.LaborDetail.Where(m => m.LaborId == model.LaborId)
                                                          on user.Id equals laborDetail.UserId into userJoinLabor
                                                          from laborDetail in userJoinLabor.DefaultIfEmpty()
                                                          orderby laborDetail.Goods
                                                          select new LaborDetailListViewModel
                                                          {
                                                              Account = user.UserName,
                                                              Department = user.Department.DeptName,
                                                              DepartmentId = user.DepartmentId,
                                                              Option = laborDetail.Option,
                                                              Goods = laborDetail.Goods,
                                                             
                                                          };
            if (model.DeptId != Guid.Empty)
            {
                result = result.Where(m => m.DepartmentId == model.DeptId);
            }
            return result;
        }

        /// <summary>
        /// 根据部门设置默认的劳保
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public async Task SetDefaultLabor(DefaultLaborViewModel model)
        {
            var userIdlist = from user in _context.User.Where(m=>m.DepartmentId==model.DeptId)
                             join laborDetail in _context.LaborDetail.Where(m => m.LaborId == model.LaborId)
                             on user.Id equals laborDetail.UserId into userJoinLabor
                             from laborDetail in userJoinLabor.DefaultIfEmpty()
                             orderby laborDetail.Goods
                             where laborDetail.Option == null
                             select new
                             {
                                 UserId = user.Id,
                                 user.UserName,
                                 laborDetail.Option
                             };

            foreach (var item in userIdlist)
            {
                await CreateAsync(new LaborDetail
                {
                    UserId = item.UserId,
                    Goods = model.Goods,
                    Option = model.Option,
                    LaborId = model.LaborId,
                }, false);
            }
            await SaveAsync();

        }
    }
}
