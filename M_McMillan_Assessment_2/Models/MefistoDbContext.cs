using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class MefistoDbContext : IdentityDbContext<User>
    {
            // Setting Tables in the DB
            public DbSet<Category> Categories { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Comment> Comments { get; set; }
            public DbSet<Message> Messages { get; set; }
            public MefistoDbContext() : base("MefistoConnection", throwIfV1Schema: false)
            {
                Database.SetInitializer(new DatabaseInitialiser());
            }

            public static MefistoDbContext Create()
            {
                return new MefistoDbContext();
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Set cascade on delete for Foreign Key Relationships
                modelBuilder.Entity<Member>().HasOptional(u => u.Comments).WithMany().WillCascadeOnDelete(false);
                modelBuilder.Entity<Employee>().HasOptional(u => u.Posts).WithMany().WillCascadeOnDelete(true);
            }
    }
}