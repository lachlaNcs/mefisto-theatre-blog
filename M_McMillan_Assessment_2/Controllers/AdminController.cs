using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using M_McMillan_Assessment_2.Models;
using M_McMillan_Assessment_2.Models.ViewModels;

namespace M_McMillan_Assessment_2.Controllers
{
    // Mark McMillan - 15/01/2024
    [Authorize(Roles = "Admin")] // Only Admins can access this Controller
    public class AdminController : AccountController
    {
        // Instance of MefistoDbContext
        private MefistoDbContext db = new MefistoDbContext();

        public AdminController()
            : base() { }

        public AdminController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager
        )
            : base(userManager, signInManager) { }

        // GET: Admin
        // The Index Action will display all Users
        public ActionResult Index()
        {
            // Get all the Users
            var users = db.Users.ToList();

            // Send the List of Users to the Index View
            return View(users);
        }

        // Create a new Employee
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            CreateEmployeeViewModel employeeVM = new CreateEmployeeViewModel();

            // Get all the Roles from the db and store them as selectedListItem so they can be displayed in a Dropdown List
            var roles = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            }).ToList();

            // Assign the Roles to the employeeVM variable
            employeeVM.Roles = roles;

            // Send the employeeVM model to the View
            return View(employeeVM);
        }

        [HttpPost]
        // POST Action for CreateEmployee
        public ActionResult CreateEmployee(CreateEmployeeViewModel model)
        {
            // If the model is not null
            if (ModelState.IsValid)
            {
                // Build Employee Object
                Employee newEmployee = new Employee
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Street = model.Street,
                    City = model.City,
                    Postcode = model.Postcode,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmploymentStatus = model.EmploymentStatus,
                    IsActive = true,
                    IsSuspended = false,
                    RegisteredAt = DateTime.Now
                };

                // Create User and store in the db and pass the password to be hashed
                var result = UserManager.Create(newEmployee, model.Password);

                // If User was stored in the db successfully
                if (result.Succeeded)
                {
                    // Then add the User to the role selected
                    UserManager.AddToRole(newEmployee.Id, model.Role);

                    return RedirectToAction("Index", "Admin");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")] // Only Admins can access this Action
        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            // If the User ID is null then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the Employee in the db by the ID
            Employee employee = db.Users.Find(id) as Employee;

            // If the Employee wasnt found, return NotFound error
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Send the Employees details to the View
            return View(new EditEmployeeViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Street = employee.Street,
                City = employee.City,
                Postcode = employee.Postcode,
                EmploymentStatus = employee.EmploymentStatus
            });
        }

        // POST: Users/EditEmployee/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmployee(string id,
            [Bind(Include = "FirstName, LastName, Street, City, Postcode, EmploymentStatus")] 
            EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = (Employee) await UserManager.FindByIdAsync(id); // Find the User by ID and Cast them to Employee type

                UpdateModel(employee); // Update the new Employee details by using the Model

                IdentityResult result = await UserManager.UpdateAsync(employee); // Update the new Employees details in the db

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditMember(string id)
        {
            // If User ID is null then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Member member = db.Users.Find(id) as Member; // Find User by ID and cast it to a Member

            // If the Member is not found in the DB then return a NotFound error
            if (member == null)
            {
                return HttpNotFound();
            }

            // Send Member Details to the View
            return View(new EditMemberViewModel
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                Street = member.Street,
                City = member.City,
                Postcode = member.Postcode,
                IsSuspended = member.IsSuspended
            });
        }
        // POST: Users/EditMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditMember(string id,
            [Bind(Include = "FirstName, LastName, Street, City, Postcode, IsSuspended")]
            EditMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get Member from db by ID
                Member member = (Member) await UserManager.FindByIdAsync(id);

                UpdateModel(member); // Updates Member Details using the Values from model

                IdentityResult result = await UserManager.UpdateAsync(member); // Update Member Details in the db

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View(model);
        }

        // GET: Users/Details/5
        // This Action gets the Details of Users
        public ActionResult Details(string id)
        {
            // If the User ID is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            
            // If the User wasn't found in the DB, then return a NotFound error
            if (user == null)
            {
                return HttpNotFound();
            }

            // If the User is an Employee object, then return the View for that Type
            if (user is Employee)
            {
                return View("DetailsEmployees", (Employee)user);
            }

            // If the User is an Member object, then return the View for that Type
            if (user is Member)
            {
                return View("DetailsMember", (Member)user);
            }

            // If we get here then no User was found in the DB and we return a NotFound error
            return HttpNotFound();
        }

        [HttpGet]
        // GET Action for Creating a Role
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        // POST Action for Creating a Role
        public ActionResult CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the RoleManager
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Check that there are no duplicate Roles stored in the db
                if (!roleManager.RoleExists(model.RoleName))
                {
                    // Create and save the new Role in the db
                    roleManager.Create(new IdentityRole(model.RoleName));

                    return RedirectToAction("Index", "Admin");
                }
            }
            // If we get here then there was an error and we return the model to the View
            return View(model);
        }

        // GET: Users/ChangeRole/5
        [HttpGet]
        public async Task<ActionResult> ChangeRole(string id)
        {
            // If we the UserID is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // This makes sure that the User can't change their own Role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Admin");
            }

            // Get User by ID
            User user = await UserManager.FindByIdAsync(id);
            // Get Users current role
            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            // Get all the Roles from the db and store them as a list of selectedListItems
            var roles = db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == oldRole
            }).ToList();

            // Build the ChangeRoleViewModel Object including the List of Roles
            // and send it to the View displaying the Roles in a DropdownList with the Users current role displayed as Selected
            return View(new ChangeRoleViewModel
            {
                UserName = user.UserName,
                Roles = roles,
                OldRole = oldRole,
            });
        }

        // POST: Users/ChangeRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public async Task<ActionResult> ChangeRoleConfirmed(string id, [Bind(Include = "Role")] ChangeRoleViewModel model)
        {
            // Can't change your own Role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Admin");
            }

            if (ModelState.IsValid)
            {
                User user = await UserManager.FindByIdAsync(id); // Get User by ID

                // Get Users current Role
                string oldRole = (await UserManager.GetRolesAsync(id)).Single(); // Only ever a single Role

                // If current Role is the same as selected Role then there is no point updating the db
                if (oldRole == model.Role)
                {
                    return RedirectToAction("Index", "Admin");
                }

                // Remove User from the old role first
                await UserManager.RemoveFromRoleAsync(id, oldRole);
                // Adding User to new Role
                await UserManager.AddToRoleAsync(id, model.Role);

                // If the User was suspended
                if (model.Role == "Suspended")
                {
                    // Then set IsSuspended to true
                    user.IsSuspended = true;

                    // Update Users details in the db
                    await UserManager.UpdateAsync(user);
                }

                return RedirectToAction("Index", "Admin");
            }
            // If we get here then there was an error and we return the model to the View
            return View(model);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            // If the UserID is null then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);

            // If no User was found in the DB then return a NotFound error
            if (user == null)
            {
                return HttpNotFound();
            }

            // Get all the Users comments
            var comments = db.Comments.Where(u => u.UserId == user.Id);
            // Get all the Users posts
            var posts = db.Posts.Where(u => u.UserId == user.Id);

            // Removing all the Users Comments
            db.Comments.RemoveRange(comments);
            // Removing all the Users Posts
            db.Posts.RemoveRange(posts);
            // Remove the User from the DB
            db.Users.Remove(user);

            // Save the changes in the DB
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        // This is the Action that will process the Search form on the Admin page
        public ViewResult Index(string SearchString)
        {
            // This means that if the user leaves the SearchBox empty, it will return a list of all the Users again
            IQueryable<User> users = db.Users;
            if (string.IsNullOrEmpty(SearchString))
            {
                users.ToList();
            }
            else
            {
                users = users.Where(u => u.UserName.Equals(SearchString.Trim())).OrderByDescending(u => u.UserName);
            }
            return View(users);
        }
    }
}
