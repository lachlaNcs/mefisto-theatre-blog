using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M_McMillan_Assessment_2.Models.ViewModels
{
    // Mark McMillan - 15/01/2024
    // This ViewModel is used by the Admin when creating a new Employee
    public class CreateEmployeeViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email Confirm")]
        public bool EmailConfirm { get; set; }
        [Display(Name = "Phone Confirm")]
        public bool PhoneConfirm { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public ICollection<SelectListItem> Roles { get; set; }
    }
}