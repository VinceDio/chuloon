using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Nickname { get; set; }

        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }

        public string StatusMessage { get; set; }
    }
}
