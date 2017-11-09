using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ggcvan.Models
{
    public class GameRank
    {
        [ForeignKey("ApplicationUserId")]
        public int ApplicationUserId { get; set; }
        [ForeignKey("GameId")]
        public int GameId { get; set; }
        public ApplicationUser User { get; set; }
        public Game Game { get; set; }
        public int Rank { get; set; }
    }
}