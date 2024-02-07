using M_McMillan_Assessment_2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M_McMillan_Assessment_2.Controllers
{
    // Mark McMillan - 15/01/2024
    public class ModeratorController : Controller
    {
        private MefistoDbContext db = new MefistoDbContext(); // Spawn an instance of the DB
        // GET: Moderator
        [Authorize(Roles = "Moderator")] // Only the Moderator Role can access this
        public ActionResult Index()
        {
            return View();
        }

        // GET: Moderator/Details/5
        // This Action gets the Details of a Category
        public ActionResult Details(int? id)
        {
            // If the CategoryID is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Category selected in the Categories table
            Category category = db.Categories.Find(id);

            // If the Category was not found in the DB, then return a NotFound error
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: Moderator/Create
        // Action used to Create a new Category
        public ActionResult Create()
        {
            return View();
        }

        // POST: Moderator/Create
        // POST Action for Creating a new Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId, CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                // Add the new Category to the Categories table
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }

            return View(category);
        }

        // GET: Moderator/Edit/5
        // GET Action for Editing a Category
        public ActionResult Edit(int? id)
        {
            // If the CategoryID is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Category in the Categories table by its ID
            Category category = db.Categories.Find(id);

            // If no Category was found in the DB, then return a NotFound error
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Moderator/Edit/5
        // POST Action for Editing a Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId, CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                // Setting the state of the DB to Modified 
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }

            return View(category);
        }

        // GET Action for Editing a Post
        public ActionResult EditPost(int? id)
        {
            // If PostId is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Post in the Posts table by its ID
            Post post = db.Posts.Find(id);

            // If no Post was found in the DB then return a NotFound error
            if (post == null)
            {
                return HttpNotFound();
            }

            // Send the Categories in a ViewBag so we can use it in the View
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // POST Action for Editing a Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "PostId, Title, Description, DatePosted, DateEdited, CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.DateEdited = DateTime.Now; // Set the DateEdited to the time of Edit submission
                post.UserId = User.Identity.GetUserId();
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllPosts");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", post.CategoryId);

            return View(post);
        }

        // Action for Deleting a Category
        public ActionResult Delete(int? id)
        {
            // If the CategoryId is null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Category in the Categories table by its ID
            Category category = db.Categories.Find(id);

            // If the Category wasnt found in the DB, then return a NotFound error
            if (category == null)
            {
                return HttpNotFound();
            }

            // Remove the category from the Categories table
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("ViewAllCategories");
        }

        // Action that returns a List of all Categories to the View
        public ActionResult ViewAllCategories()
        {
            return View(db.Categories.ToList());
        }

        // Action that returns a List of all Posts to the View
        public ActionResult ViewAllPosts()
        {
            List<Post> posts = db.Posts
                .Include(p => p.Category)
                .Include(p => p.Employee)
                .Include(p => p.Comments)
                .ToList();

            return View(posts);
        }

        // Action used when Mod deletes a Post
        public ActionResult DeletePost(int? id)
        {
            // If the PostId was null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            
            // Find the Post in the Posts table by its ID
            Post post = db.Posts.Find(id);

            // If the Post wasnt found in the DB, then return a NotFound error
            if (post == null)
            {
                return HttpNotFound();
            }

            // Get the Author of the Post by getting the UserId attached to the Post
            var author = db.Users.Find(post.UserId);

            // Get the Category of the Post by getting the CategoryId attached to the Post
            var category = db.Categories.Find(post.CategoryId);

            // Set the Employee Property in the Post class to the Author of the Post from the DB
            post.Employee = (Employee)author;

            // Set the Category Property in the Post class to the Category of the Post from the DB
            post.Category = category;

            // Remove the Post from the Posts table
            db.Posts.Remove(post);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts");
        }
 
        // Action that returns a List of all Comments from the Comments table
        public ActionResult ViewAllComments()
        {
            List<Comment> comments = db.Comments.Include(c => c.Post).Include(c => c.User).ToList();

            return View(comments);
        }

        // Action used when Mod deletes a Comment
        public ActionResult DeleteComment(int? id)
        {
            // If CommentId was null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult (System.Net.HttpStatusCode.BadRequest);
            }

            // Find the Comment in the Comments table by its ID
            Comment comment = db.Comments.Find(id);

            // If the Comment wasnt found in the Comments table, then return a NotFound error
            if (comment == null)
            {
                return HttpNotFound();
            }

            // Remove the Comment from the Comments table
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("ViewAllComments");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
