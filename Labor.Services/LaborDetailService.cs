using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Services
{
    public class LaborDetailService:BaseService<LaborDetail>,ILaborDetailService
    {
        private readonly ILaborDetailRepository _laborDetailRepository;
        public LaborDetailService(ILaborDetailRepository laborDetailRepository)
        {
            _laborDetailRepository = laborDetailRepository;
            BaseRepository = laborDetailRepository;
        }
    }
}
