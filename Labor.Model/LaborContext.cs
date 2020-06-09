using Labor.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Labor.Model
{
    public class LaborContext:DbContext
    {
        public LaborContext() { }

        public LaborContext(DbContextOptions<LaborContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<LaborHead> LaborHead { get; set; }
        public DbSet<LaborDetail> LaborDetail { get; set; }

    }
}
