using System.ComponentModel.DataAnnotations;
namespace ggcvan.Models
{
    public class GameRank
    {
        public int ApplicationUserId { get; set; }
        public int GameId { get; set; }
        public ApplicationUser User { get; set; }
        public Game Game { get; set; }
        public int Rank { get; set; }
    }
}