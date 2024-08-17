#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LaserCraftHub.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage = "You can't post empty message.")]
        [Display(Name = "Post a message")]
        public string MessageText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Comment> Comments { get; set; } = [];

        public int CraftId { get; set; }
        public Craft? Craft { get; set; }
    }
}