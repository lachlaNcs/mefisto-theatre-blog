using System.Data.Entity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public abstract class User : IdentityUser // User inherits from IdentityUser Class
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        [Display(Name = "Post Code")]
        public string Postcode { get; set; }
        [Display(Name = "Joined")]
        public DateTime RegisteredAt { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }

        private ApplicationUserManager userManager;

        // Navigational Properties
        public List<Comment> Comments { get; set; }

        [NotMapped] // This Data Annotation prevents the CurrentRole from being mapped in the DB
        [Display(Name = "Role")]
        public string CurrentRole
        {
            get
            {
                if (userManager == null)
                {
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }

                return userManager.GetRoles(Id).Single();
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}