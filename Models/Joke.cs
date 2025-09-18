using System.ComponentModel.DataAnnotations;

namespace GroanZone.Models
{
    public class Joke
    {
        public int JokeId { get; set; }

        [Required, MinLength(2)]
        public string Setup { get; set; } = "";

        [Required, MinLength(2)]
        public string Punchline { get; set; } = "";

        public int UserId { get; set; }
        public User? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Rating> Ratings { get; set; } = [];
    }
}