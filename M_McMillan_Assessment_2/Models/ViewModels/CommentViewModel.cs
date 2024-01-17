using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models.ViewModels
{
    // Mark McMillan - 15/01/2024
    // This ViewModel is used by the User when creating a Comment on a Post
    public class CommentViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}