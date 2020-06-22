using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Repository
{
    public class DepartmentRepository:BaseRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(LaborContext context) : base(context) { }
    }
}
