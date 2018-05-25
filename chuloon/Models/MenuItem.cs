using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class MenuItem
    {
        public MenuItem()
        {
            Prices = new List<MenuPrice>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        [Display(Name="Menu Category")]
        public int MenuCategoryId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }

        [ForeignKey("MenuCategoryId")]
        public MenuCategory MenuCategory { get; set; }

        public List<MenuPrice> Prices { get; set; }

    }
}
