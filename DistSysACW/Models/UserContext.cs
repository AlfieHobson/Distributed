using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base()
        {

        }

        public DbSet<User> Users { get; set; }

        //TODO: Task13

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DistSysACW;");
        }
    }
}