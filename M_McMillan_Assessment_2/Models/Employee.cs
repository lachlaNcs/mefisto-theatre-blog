using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class Employee : User // Employee inherits from User
    {
        // Employee Properties
        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

        // Navigational Properties
        public List<Post> Posts { get; set; }
    }

    public enum EmploymentStatus
    {
        FullTime,
        PartTime
    }
}