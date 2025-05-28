using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Influencerhub.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Influencerhub.DAL.Data
{
    public class InfluencerhubDBContext : DbContext
    {
        public InfluencerhubDBContext() { }
        public DbSet<Models.User> Users { get; set; } = null!;
        public DbSet<Models.Role> Roles { get; set; } = null!;
        public DbSet<Models.Influ> Influs { get; set; } = null!;
        public DbSet<Models.Business> Businesses { get; set; } = null!;
        public DbSet<Models.BusinessField> BusinessFields { get; set; } = null!;
        public DbSet<Models.Field> Fields { get; set; } = null!;
        public DbSet<Models.FreelanceField> FreelanceFields { get; set; } = null!;
        public DbSet<Models.FreelanceJob> FreelanceJobs { get; set; } = null!;
        public DbSet<Models.Job> Jobs { get; set; } = null!;
        public DbSet<Models.Membership> Memberships { get; set; } = null!;
        public DbSet<Models.MembershipType> MembershipTypes { get; set; } = null!;
        public DbSet<Models.Representative> Representatives { get; set; } = null!;
        public DbSet<Models.Review> Reviews { get; set; } = null!;
        public DbSet<Models.Transaction> Transactions { get; set; } = null!;
        public DbSet<Models.Link> Links { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-J0IBK4L\\TUT;database=InfluencerhubDB;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
    }
}