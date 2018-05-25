using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class MenuPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [MaxLength(100)]
        public string Note { get; set; }

        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; }

    }
}
