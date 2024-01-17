using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class Post
    {
        // Post Properties
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] // Format as ShortDateTime
        public DateTime? DatePosted { get; set; }

        [Display(Name = "Date Edited")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateEdited { get; set; }

        // Navigational properties
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Employee")]
        public string UserId { get; set; }
        public Employee Employee { get; set; }

        public List<Comment> Comments { get; set; }
    }
}