namespace Upgrading
{
    public static class LevelsPartCalculator
    {
        public static int CalculateSuperLevel(int level)
        {
            if (level == 1)
                return 1;
            return (level + 2) / UpgradeUnitsConstants.LevelsToUpgradePart;
        }

        public static int CalculateNextSuperLevel(int level) => CalculateSuperLevel(level) + 1;

        public static int CalculateLevelsAmountToReachSuperLevel(int superLevel)
        {
            if (superLevel == 1)
                return 0;
            return (superLevel - 1) * UpgradeUnitsConstants.LevelsToUpgradePart;
        }

        public static int HowMuchLevelsToCompleteToReachNextSuperLevel(int currentLevel)
        {
            if (currentLevel % UpgradeUnitsConstants.LevelsToUpgradePart == 0)
                return 3;
            return CalculateLevelsAmountToReachSuperLevel(CalculateNextSuperLevel(currentLevel)) - currentLevel;
        }

        public static int HowMuchLevelsToCompleteToReachSuperLevel(int currentLevel, int targetSuperLevel)
        {
            return CalculateLevelsAmountToReachSuperLevel(targetSuperLevel) - currentLevel;
        }
    }
}