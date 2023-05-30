using MathUtility;

namespace Gameplay.Levels.Domain
{
    public static class RewardCalculator
    {
        private const float LevelRewardGrowMultiplier = 1.1f;
        private const int BaseLevelRewardAmount = 5;

        public static int RewardForLevel(int level)
        {
            return (int) Functions.Polynomial(
                x: level, 
                n: LevelRewardGrowMultiplier,
                add: BaseLevelRewardAmount);
        }
    }
}