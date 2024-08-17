#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LaserCraftHub.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "You can't post empty comment")]
        [Display(Name = "Post a comment")]
        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int MessageId { get; set; }
        public Message? Message { get; set; }
    }
}