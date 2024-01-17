using M_McMillan_Assessment_2.Models;
using M_McMillan_Assessment_2.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M_McMillan_Assessment_2.Controllers
{
    // Mark McMillan - 15/01/2024
    public class HomeController : Controller
    {
        private MefistoDbContext context = new MefistoDbContext(); // Spawn an instance of the DB
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        // This is the Post Action for when the User submits the Contact form
        [HttpPost]
        public ActionResult Contact(Message message)
        {
            if (ModelState.IsValid)
            {
                message.DateOfMessage = DateTime.Now; // Set the Message Date to the Date it was when they submitted 
                context.Messages.Add(message); // Add the Message to the Message table
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }
        
        // This Action gets all the Blogs from the DB and sends it to the View
        public ActionResult GetBlogs()
        {
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Employee).Include(p => p.Comments).OrderByDescending(p => p.DatePosted);

            ViewBag.Categories = context.Categories.ToList();

            return View(posts.ToList());
        }

        // This ViewResult is used when the User searches for a Category on the Blogs page
        [HttpPost]
        public ViewResult GetBlogs(string SearchString)
        {
            IQueryable<Post> posts = context.Posts
                .Include(p => p.Category)
                .Include(p => p.Employee)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.DatePosted);

            // This is done so that if the User submits an empty SearchBox, we return a list of all the posts
            if (string.IsNullOrEmpty(SearchString))
            {
                posts.ToList();
            }
            else
            {
                posts = context.Posts
                .Include(p => p.Category)
                .Include(p => p.Employee)
                .Where(p => p.Category.CategoryName.Equals(SearchString.Trim())) // WHERE Clause for getting the CategoryName that equals the Users inputed SearchString
                .OrderByDescending(p => p.DatePosted);
            }

            ViewBag.Categories = context.Categories.ToList();

            return View(posts);
        }

        // This is the Action for getting the Details of a Post
        public ActionResult Details(int? id)
        {
            // Search the Posts table in the db
            // and get the post by its ID
            Post post = context.Posts.Find(id);

            // Using the Foreign Key UserId from the Post instance
            // to get the Employee who created the Post
            var employee = context.Users.Find(post.UserId);
            
            // Using the Foreign Key CategoryId 
            // Find the Category that the Post belongs to
            var category = context.Categories.Find(post.CategoryId);

            var comments = context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == id)
                .ToList();

            // Find(post.PostId == id)
            // Assigning the employee to the Employee Navigational Property in Post
            post.Employee = (Employee)employee;

            // Assign the Category to the Category Navigational Property in Post
            post.Category = category;

            post.Comments = comments;
            // Send the Post model to the Details View
            return View(post);
        }
    }
}