using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class Category
    {
        // Category Properties
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        // Navigational Property
        public List<Post> Posts { get; set; }
    }
}