using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Repository
{
    public class LaborHeadRepository : BaseRepository<LaborHead>, ILaborHeadRepository
    {
        public LaborHeadRepository(LaborContext context) : base(context) { }
    }
}
