using System.Linq;
using GameBalance;

namespace Gameplay.Levels
{
    public static class LevelExtensions
    {
        public static int EnemiesAmount(this LevelEntry level) => 
            level.Waves.Sum(x => x.Enemies.Sum(x => x.Quantity));
    }
}