using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace chuloon.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }
        public string ImageURL { get; set; }

        public string NicknameOrUser {
            get
            {
                return Nickname ?? UserName;
            }
        }

        public string ImageOrDefault
        {
            get
            {
                return ImageURL ?? "/images/vador.jpg";
            }
        }
    }
}
