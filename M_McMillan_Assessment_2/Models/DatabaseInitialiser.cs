using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class DatabaseInitialiser : DropCreateDatabaseAlways<MefistoDbContext>
    {
        protected override void Seed(MefistoDbContext context)
        {
            base.Seed(context);

            if (!context.Users.Any())
            {
                // Create some Roles and store them in the Roles table
                // We use the RoleManager to do this
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // If the Role doesn't exist then we create the Role
                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                if (!roleManager.RoleExists("Moderator"))
                {
                    roleManager.Create(new IdentityRole("Moderator"));
                }

                if (!roleManager.RoleExists("Staff"))
                {
                    roleManager.Create(new IdentityRole("Staff"));
                }

                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }

                if (!roleManager.RoleExists("Suspended"))
                {
                    roleManager.Create(new IdentityRole("Suspended"));
                }

                // If we want to create Users - Members or Employees - we need a UserManager
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                // Creating an Admin - Check if the Admin already exists in the DB first
                userManager.PasswordValidator = new PasswordValidator
                {
                   RequireDigit = false,
                   RequiredLength = 1,
                   RequireLowercase = false,
                   RequireUppercase = false,
                   RequireNonLetterOrDigit = false
                };

                var admin = new Employee
                {
                   UserName = "admin@mefisto.com",
                   Email = "admin@mefisto.com",
                   FirstName = "Mark",
                   LastName = "McMillan",
                   Street = "190 Cathedral St",
                   City = "Glasgow",
                   Postcode = "G4 0RF",
                   RegisteredAt = DateTime.Now.AddYears(-3),
                   EmailConfirmed = true,
                   IsActive = true,
                   IsSuspended = false,
                   EmploymentStatus = EmploymentStatus.FullTime
                };

                if (userManager.FindByName("admin@mefisto.com") == null)
                {
                    // Add the Admin to the Users table
                    // Last parameter is the Password
                    userManager.Create(admin, "admin123");
                    // Assign it to the Admin Role
                    userManager.AddToRole(admin.Id, "Admin");
                }

                // Creating a Moderator
                var mod = new Employee
                {
                    UserName = "moderator@mefisto.com",
                    Email = "moderator@mefisto.com",
                    FirstName = "Lachlan",
                    LastName = "McMillan",
                    Street = "128 Hillkirk St",
                    City = "Glasgow",
                    Postcode = "G21 3TH",
                    RegisteredAt = DateTime.Now.AddYears(-3),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                    EmploymentStatus = EmploymentStatus.FullTime
                };

                if (userManager.FindByName("moderator@mefisto.com") == null)
                {
                    userManager.Create(mod, "mod123");
                    userManager.AddToRole(mod.Id, "Moderator");
                }

                // Creating a Staff
                var staff = new Employee
                {
                    UserName = "staff@mefisto.com",
                    Email = "staff@mefisto.com",
                    FirstName = "Hayley",
                    LastName = "McDonald",
                    Street = "90 Hope St",
                    City = "Glasgow",
                    Postcode = "G42 9DT",
                    RegisteredAt = DateTime.Now.AddYears(-3),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false,
                    EmploymentStatus = EmploymentStatus.PartTime
                    };

                if (userManager.FindByName("staff@mefisto.com") == null)
                {
                    userManager.Create(staff, "staff123");
                    userManager.AddToRole(staff.Id, "Staff");
                }

                // Creating a few Members
                 var member = new Member
                 {
                    UserName = "james@gmail.com",
                    Email = "james@gmail.com",
                    FirstName = "James",
                    LastName = "Tav",
                    Street = "123 Queen St",
                    City = "Glasgow",
                    Postcode = "G56 8FG",
                    RegisteredAt = DateTime.Now.AddMonths(-5),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false
                 };

                if (userManager.FindByName("james@gmail.com") == null)
                {
                    userManager.Create(member, "member123");
                    userManager.AddToRole(member.Id, "Member");
                }

                var john = new Member
                {
                    UserName = "john@gmail.com",
                    Email = "john@gmail.com",
                    FirstName = "John",
                    LastName = "Smith",
                    Street = "15 Duke St",
                    City = "Glasgow",
                    Postcode = "G21 2DW",
                    RegisteredAt = DateTime.Now.AddYears(-1),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false
                };

                if (userManager.FindByName("john@gmail.com") == null)
                {
                    userManager.Create(john, "john123");
                    userManager.AddToRole(john.Id, "Member");
                }

                var scott = new Member
                {
                    UserName = "scott@gmail.com",
                    Email = "scott@gmail.com",
                    FirstName = "Scott",
                    LastName = "McMillan",
                    Street = "12 Rottenrow St",
                    City = "Glasgow",
                    Postcode = "G21 2DF",
                    RegisteredAt = DateTime.Now.AddMonths(-10),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false
                };

                if (userManager.FindByName("scott@gmail.com") == null)
                {
                    userManager.Create(scott, "scott123");
                    userManager.AddToRole(scott.Id, "Member");
                }

                var barry = new Member
                {
                    UserName = "barry@gmail.com",
                    Email = "barry@gmail.com",
                    FirstName = "Barry",
                    LastName = "Ferguson",
                    Street = "12 Govan St",
                    City = "Glasgow",
                    Postcode = "G27 9BH",
                    RegisteredAt = DateTime.Now.AddMonths(-2),
                    EmailConfirmed = true,
                    IsActive = true,
                    IsSuspended = false
                };

                if (userManager.FindByName("barry@gmail.com") == null)
                {
                    userManager.Create(barry, "barry123");
                    userManager.AddToRole(barry.Id, "Member");
                }

                // Creating a Suspended Member
                var troll = new Member
                {
                     UserName = "troll@gmail.com",
                     Email = "troll@gmail.com",
                     FirstName = "Online",
                     LastName = "Troll",
                     Street = "6 Hopeless St",
                     City = "Edinburgh",
                     Postcode = "G9 7DF",
                     RegisteredAt = DateTime.Now.AddDays(-1),
                     EmailConfirmed = true,
                     IsActive = false,
                     IsSuspended = true
                };

                if (userManager.FindByName("troll@gmail.com") == null)
                {
                    userManager.Create(troll, "suspended123");
                    userManager.AddToRole(troll.Id, "Suspended");
                }
                context.SaveChanges();

                // Creating some categories
                var cat1 = new Category() { CategoryName = "Announcement" };
                var cat2 = new Category() { CategoryName = "Movie Review" };
                var cat3 = new Category() { CategoryName = "Performance Review" };

                // Add them to the Category table
                context.Categories.Add(cat1);
                context.Categories.Add(cat2);
                context.Categories.Add(cat3);

                context.SaveChanges();

                // Seeding in Posts
                var post1 = new Post()
                {
                    Title = "Les Miserables",
                    Description = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 1, 1, 8, 0, 15),
                    DateEdited = new DateTime(2019, 1, 2, 8, 0, 15),
                    Category = cat3,
                    Employee = admin
                };

                var post2 = new Post()
                {
                    Title = "The Sorcerers Apprentice",
                    Description = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 5, 25, 8, 0, 15),
                    DateEdited = null,
                    Category = cat2,
                    Employee = mod
                };

                var post3 = new Post()
                {
                    Title = "New Play Coming Soon",
                    Description = "Lorem ipsu...",
                    DatePosted = new DateTime(2019, 6, 1, 6, 0, 15),
                    DateEdited = null,
                    Category = cat1,
                    Employee = staff
                };

                context.Posts.Add(post1);
                context.Posts.Add(post2);
                context.Posts.Add(post3);

                // Creating some Comments
                var comment1 = new Comment()
                {
                    Content = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 1, 1, 9, 0, 15),
                    Post = post1,
                    User = member
                };

                var comment2 = new Comment()
                {
                    Content = "Lorem ipsum 2...",
                    DatePosted = new DateTime(2019, 1, 1, 10, 0, 15),
                    Post = post1,
                    User = john
                };

                var comment3 = new Comment()
                {
                    Content = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 5, 25, 9, 0, 15),
                    Post = post2,
                    User = barry
                };

                var comment4 = new Comment()
                {
                    Content = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 5, 25, 9, 0, 15),
                    Post = post2,
                    User = john
                };

                var comment5 = new Comment()
                {
                    Content = "Lorem ipsum...",
                    DatePosted = new DateTime(2019, 6, 1, 8, 0, 15),
                    Post = post3,
                    User = barry
                };
                
                // Adding the comments to the Comments table
                context.Comments.Add(comment1);
                context.Comments.Add(comment2);
                context.Comments.Add(comment3);
                context.Comments.Add(comment4);
                context.Comments.Add(comment5);

                // Saving the DB
                context.SaveChanges();

            }// End of if any Users
        }// End of Seed Method
    }// End of Class
}// End of Namespace