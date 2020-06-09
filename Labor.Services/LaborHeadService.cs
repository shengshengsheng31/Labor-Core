using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Services
{
    public class LaborHeadService:BaseService<LaborHead>,ILaborHeadService
    {
        private readonly ILaborHeadRepository _laborHeadRepository;
        public LaborHeadService(ILaborHeadRepository laborHeadRepository)
        {
            _laborHeadRepository = laborHeadRepository;
            BaseRepository = laborHeadRepository;
        }
    }
}
