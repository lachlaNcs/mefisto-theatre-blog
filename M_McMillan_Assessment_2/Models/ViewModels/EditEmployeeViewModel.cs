using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models.ViewModels
{
    // Mark McMillan - 15/01/2024
    // This ViewModel is used by the User/Admin when Editing an Employees account
    public class EditEmployeeViewModel
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
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

    }
}