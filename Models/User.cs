using System.ComponentModel.DataAnnotations;

namespace GroanZone.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MinLength(2)]
        public string Username { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(8)]
        public string PasswordHash { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Joke> Jokes { get; set; } = new();
        public List<Rating> Ratings { get; set; } = new();
    }
}