using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            MenuItems = new List<MenuItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string Url { get; set; }
        public string ImageUrl { get; set; }

        public List<MenuItem> MenuItems { get; set; }
    }
}
