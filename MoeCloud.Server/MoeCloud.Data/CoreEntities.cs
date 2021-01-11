using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoeCloud.Data
{
    public class CoreEntities : DbContext
    {
        public CoreEntities(DbContextOptions<CoreEntities> options) : base(options) { }
        public DbSet<Model.File> Files { get; set; }
        public DbSet<Model.Role> Roles { get; set; }
        public DbSet<Model.Share> Shares { get; set; }
        public DbSet<Model.User> Users { get; set; }
        public DbSet<Model.Site> Sites { get; set; }
        public DbSet<Model.Reg> Regs { get; set; }
        public DbSet<Model.Mail> Mails { get; set; }
    }
}