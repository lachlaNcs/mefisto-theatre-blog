using M_McMillan_Assessment_2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using M_McMillan_Assessment_2.Models.ViewModels;

namespace M_McMillan_Assessment_2.Controllers
{
    // Mark McMillan - 15/01/2024
    public class StaffController : Controller
    {
        // instance of db
        private MefistoDbContext db = new MefistoDbContext();
        private ApplicationUserManager _userManager;

        public StaffController()
        {
        }

        public StaffController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Staff
        public ActionResult Index()
        {
            // Select all the Posts from the Posts table
            // .Include the foreign keys Category and Employee 
            var posts = db.Posts
                .Include(p => p.Category)
                .Include(p => p.Employee);

            // Get the ID of the logged in Employee using Identity
            var userId = User.Identity.GetUserId();

            // From the list of Posts 
            // Select only the ones that have the UserId equal to the ID of the logged in Employee
            posts = posts.Where(p => p.UserId == userId);

            // Send the List of Posts to the View
            return View(posts.ToList());
        }

        // GET: Staff/Details/5
        // Gets the Details of a Post
        public ActionResult Details(int? id)
        {
            // If the PostId was null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find Post in the Posts table by PostId
            Post post = db.Posts.Find(id);

            // If post doesn't exist then return a not found error
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // GET: Staff/Create
        // Get Action for Creating a Post
        public ActionResult Create()
        {
            // Send the List of Categories to the View using a ViewBag
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");

            return View();
        }

        // POST: Staff/Create
        // POST Action for Creating a Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId, Title, Description, DatePosted, DateEdited, CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.DatePosted = DateTime.Now; // Set the DatePosted to the time of creation
                post.DateEdited = null; // Set the DateEdited property to null as the Post hasnt been EDITED yet
                post.UserId = User.Identity.GetUserId(); // Get the UserId and assign it to UserId property in Post

                // Add the Post to the Posts table
                db.Posts.Add(post);

                // Save the changes to the DB
                db.SaveChanges();

                return RedirectToAction("GetBlogs", "Home");
            }

            // If we get here then there was a problem and we return the Categories to the View using a ViewBag
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // GET: Staff/Edit/5
        // Get Action for Editing a Post
        public ActionResult Edit(int? id)
        {
            // If PostId is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find Post by PostId in the Posts table
            Post post = db.Posts.Find(id);

            // If the Post wasnt found in the Posts table, then return a NotFound error
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // POST: Staff/Edit/5
        // Post Action for Editing a Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId, Title, Description, DatePosted, DateEdited, CategoryId")] Post post)
        {
            // If the Post that is passed is valid
            // then the edited post will be updated in the db
            if (ModelState.IsValid)
            {
                // Record the new Date of when the Post was edited
                post.DateEdited = DateTime.Now;

                // Gets the Employee ID that is logged into the system
                // and assigns it as a Foreign Key in the Post
                post.UserId = User.Identity.GetUserId();

                // Updates the db
                db.Entry(post).State = EntityState.Modified;

                // Saves the changes to the db
                db.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = post.PostId });
            }

            // If the Post is not valid, then send a list of Categories back to the Edit form
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            // Return the Post to the Edit form
            return View(post);
        }

        // GET: Staff/Delete/5
        // GET Action for Deleting a Post
        public ActionResult Delete(int? id)
        {
            // If PostId is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Post in the Posts table by its ID
            Post post = db.Posts.Find(id);

            // If the Post wasnt found in the Posts table then return a NotFound error
            if (post == null)
            {
                return HttpNotFound();
            }

            // Get the Category of the post from its CategoryId
            var category = db.Categories.Find(post.CategoryId);

            // Get the Author of the Post from its UserId
            var author = db.Users.Find(post.UserId);

            // Assign the Category property
            post.Category = category;
            // Assign the Employee property
            post.Employee = (Employee)author;

            // Remove the Post from the Posts table
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");   
        }
    }
}
