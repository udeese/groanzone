using System.ComponentModel.DataAnnotations;
using GroanZone.Models;

namespace GroanZone.ViewModels
{
    public class JokeFormVM
    {
        public int? JokeId { get; set; }

        [Required, MinLength(2)]
        public string Setup { get; set; } = "";

        [Required, MinLength(2)]
        public string Punchline { get; set; } = "";
    }

    public class JokeDetailsVM
    {
        public Joke Joke { get; set; } = default!;
        public double AverageRating { get; set; }
        public bool HasUserRated { get; set; }
        public int? ExistingUserRating { get; set; }
    }
}