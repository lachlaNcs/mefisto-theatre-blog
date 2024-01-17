using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models.ViewModels
{
    // Mark McMillan - 15/01/2024
    // This ViewModel is used by the User/Admin when Editing a Member account
    public class EditMemberViewModel
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
        [Display(Name = "Post Code")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }
    }
}