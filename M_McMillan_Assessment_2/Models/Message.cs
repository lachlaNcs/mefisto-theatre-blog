using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M_McMillan_Assessment_2.Models
{
    // Mark McMillan - 15/01/2024
    // This Class is used for the Contact Us form
    public class Message
    {
        // Message Properties
        [Key]
        public int MessageId { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfMessage { get; set; }
    }
}