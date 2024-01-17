using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M_McMillan_Assessment_2.Models.ViewModels
{
    // Mark McMillan - 15/01/2024
    // This ViewModel is used by the Admin when changing the Users Role
    public class ChangeRoleViewModel
    {
        public string UserName { get; set; }
        public string OldRole { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }

        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }
}