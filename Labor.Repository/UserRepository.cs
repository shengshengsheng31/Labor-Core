using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Labor.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LaborContext context) : base(context) { }


    }
}
