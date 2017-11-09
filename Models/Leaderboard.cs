using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace ggcvan.Models
{
    public class Leaderboard
    {
        [Key]
        public int GameId { get; set; }
        public ICollection<GameRank> GameRanks { get; set; }   
    }
}