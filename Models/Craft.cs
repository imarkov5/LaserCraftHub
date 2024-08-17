#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaserCraftHub.Models
{
    public class Craft
    {
        [Key]
        public int CraftId { get; set; }

        [Display(Name = "Craft name")]
        [Required(ErrorMessage = "Please enter your craft name.")]
        [MinLength(3, ErrorMessage = "Name must be at least three characters.")]
        public string Name { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Name must be at least three characters.")]
        public string Description { get; set; }

        public string? Website { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Like> Likes { get; set; } = [];

        public List<Message> Messages { get; set; } = [];

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool isLiked(int userId)
        {
            return Likes.Any(l => l.UserId == userId);
        }

    }
}