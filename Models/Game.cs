using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ggcvan.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string GameDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}