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
        public InfluencerhubDBContext(DbContextOptions<InfluencerhubDBContext> options) : base(options) { }
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
        public DbSet<Models.Conversation> Conversations { get; set; } = null!;
        public DbSet<Models.ConversationPartners> ConversationPartners { get; set; } = null!;
        public DbSet<Models.Message> Messages { get; set; } = null!;
        public DbSet<Models.PartnerShip> PartnerShips { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=DESKTOP-J0IBK4L\\TUT;database=InfluencerhubDB;uid=sa;pwd=12345;TrustServerCertificate=True;MultipleActiveResultSets=True;");

            optionsBuilder.UseSqlServer("Server=tcp:influencerhub1.database.windows.net,1433;Initial Catalog=influencerhub;Persist Security Info=False;User ID=sa-admin;Password=12345678a@;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

            //optionsBuilder.UseSqlServer("server=ALOLINNE;database=InfluencerhubDB;uid=sa;pwd=123;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }





    }
}