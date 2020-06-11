using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labor.Repository
{
    public class LaborHeadRepository : BaseRepository<LaborHead>, ILaborHeadRepository
    {
        private readonly LaborContext _context;
        public LaborHeadRepository(LaborContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllLabor()
        {
            _context.LaborHead.FromSqlRaw("");
            return null;
        }
    }
}
