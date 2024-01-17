using M_McMillan_Assessment_2.Models.ViewModels;
using M_McMillan_Assessment_2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Net.Http.Headers;

namespace M_McMillan_Assessment_2.Controllers
{
    public class MemberController : Controller
    {
        private MefistoDbContext context = new MefistoDbContext(); // Spawning an instance of the DB

        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        // This Action is used when a User creates a Comment under a Post
        public ActionResult Comment(int id)
        {
            ViewBag.PostId = id; // Sending the PostId in a ViewBag so we can get it on the View
            return View(new CommentViewModel());
        }

        [HttpPost]
        public ActionResult Comment(int id, CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = model.Content,
                    DatePosted = DateTime.Now,
                    UserId = User.Identity.GetUserId(),
                    PostId = id
                };

                // Adding Comment to the Comments table
                context.Comments.Add(comment);

                // Save the changes to the DB
                context.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = comment.PostId }); // Adding the routeValue for the PostId means the User will be taken back to the Post they commented on after submitting the comment
            }

            return View(model);
        }

        // Action for Editing a Comment
        public ActionResult EditComment(int id)
        {
            // Find the Comment in the Comments table by the ID
            var comment = context.Comments.Find(id);

            // Getting the PostId from the Comment
            int postId = comment.PostId;

            // Sending the PostId in a ViewBag so we can use it in the View
            ViewBag.PostId = postId;

            // If the Comment wasn't found in the DB, then return a NotFound error
            if (comment == null)
            {
                return HttpNotFound();
            }

            // This is so that when the Edit ViewModel is shown to the User, the content is filled with their Comment
            return View(new EditCommentViewModel { Content = comment.Content });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(int id, EditCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the Comment in the Comments table
                var comment = context.Comments.Find(id);

                // If the Comment wasnt found then return a NotFound error
                if (comment == null)
                {
                    return HttpNotFound();
                }

                // Set the Content property in the Comment to the Content from the Model
                comment.Content = model.Content;
                context.SaveChanges();

                return RedirectToAction("Details", "Home", new { id = comment.PostId }); // Adding this routeValue means the User is taken to the Post they edited their Comment on
            }

            return View(model);
        }

        // Action for User deleting a Comment under a Post
        public ActionResult DeleteComment(int? id)
        {
            // If the CommentID was null, then return a BadRequest error
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the Comment in the Comments table by its ID
            Comment comment = context.Comments.Find(id);

            // If the Comment wasnt found in the Comments table, then return a NotFound error
            if (comment == null)
            {
                return HttpNotFound();
            }

            // Get the PostId that the Comment is under and save it to the postId variable
            var postId = comment.PostId;
            // Remove the Comment from the Comments DB
            context.Comments.Remove(comment);
            context.SaveChanges();
            return RedirectToAction("Details", "Home", new { id = postId }); // Adding this routeValue means the User is taken back to the Post they deleted their comment from
        }
    }
}