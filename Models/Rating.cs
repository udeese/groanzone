using System.ComponentModel.DataAnnotations;

namespace GroanZone.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        [Range(1, 4)]
        public int Value { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int JokeId { get; set; }
        public Joke? Joke { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}