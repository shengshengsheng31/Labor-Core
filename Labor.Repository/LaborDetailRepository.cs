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
                                                              Account = user.Account,
                                                              Option = laborDetail.Option,
                                                              Goods = laborDetail.Goods
                                                          };
            return result;
        }
    }
}
