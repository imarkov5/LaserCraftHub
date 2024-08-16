using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaserCraftHub.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int CraftId { get; set; }
        public Craft? Craft { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}