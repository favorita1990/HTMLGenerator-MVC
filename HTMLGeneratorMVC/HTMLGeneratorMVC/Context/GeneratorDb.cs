using System.Data.Entity;
using HTMLGeneratorMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HTMLGeneratorMVC.Context
{

    public class GeneratorDb : IdentityDbContext<UserModel, Identity2Role,
    int, UserLogin, UserRole, UserClaim>
    {
        public GeneratorDb()
            : base("HTMLGeneratorMVC")
        {
        }

        public static GeneratorDb Create()
        {
            return new GeneratorDb();
        }

        public virtual DbSet<Style> Styles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Style>().ToTable("Styles");

            base.OnModelCreating(modelBuilder);
        }
    }
}