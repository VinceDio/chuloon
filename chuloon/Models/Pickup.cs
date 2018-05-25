using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class Pickup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int RestaurantId { get; set; }

        [MaxLength(450)]
        public string PickupUserId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime PickupTime { get; set; }

        public string Note { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(PickupUserId))]
        public ApplicationUser PickupUser { get; set; }

        public List<Order> Orders { get; set; }
    }
}
