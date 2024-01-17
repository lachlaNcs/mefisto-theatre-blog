using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    public class Comment
    {
        // Comment Properties
        [Key]
        public int CommentId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DatePosted { get; set; }

        // Navigational Properties
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}