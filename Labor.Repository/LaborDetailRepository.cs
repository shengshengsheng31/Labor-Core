using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Repository
{
    public class LaborDetailRepository : BaseRepository<LaborDetail>, ILaborDetailRepository
    {
        public LaborDetailRepository(LaborContext context):base(context) { }
    }
}
