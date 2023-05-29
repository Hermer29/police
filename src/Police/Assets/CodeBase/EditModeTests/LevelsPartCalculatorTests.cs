using NUnit.Framework;
using static Upgrading.LevelsPartCalculator;

namespace CodeBase.EditModeTests
{
    public class LevelsPartCalculatorTests
    {
        [TestCase(1, 1)]
        [TestCase(4, 2)]
        [TestCase(0, 0)]
        [TestCase(6, 2)]
        [TestCase(9, 3)]
        public void CalculateSuperLevelTest(
            int currentLevel, int expectedSuperLevel)
        {
            //act
            var superLevel = CalculateSuperLevel(currentLevel);

            //assert
            Assert.AreEqual(expectedSuperLevel, superLevel);
        }

        [TestCase(1, 2)]
        [TestCase(4, 3)]
        [TestCase(0, 1)]
        [TestCase(6, 3)]
        [TestCase(9, 4)]
        public void CalculateNextSuperLevelTest(int currentLevel, int expectedSuperLevel)
        {
            //act
            var superLevel = CalculateNextSuperLevel(currentLevel);

            //assert
            Assert.AreEqual(expectedSuperLevel, superLevel);
        }
        
        [TestCase(1, 0)]
        [TestCase(4, 9)]
        [TestCase(6, 15)]
        [TestCase(9, 24)]
        public void CalculateLevelsAmountToReachSuperLevelTest(int soughtSuperLevel, int expectedQuantity)
        {
            //act
            var levels = CalculateLevelsAmountToReachSuperLevel(soughtSuperLevel);

            //assert
            Assert.AreEqual(expectedQuantity, levels);
        }
        
        [TestCase(1, 2)]
        [TestCase(4, 2)]
        [TestCase(8, 1)]
        [TestCase(12, 3)]
        [TestCase(15, 3)]
        public void HowMuchLevelsToCompleteToReachNextSuperLevelTest(int currentLevel, int expectedQuantity)
        {
            //act
            var levels = HowMuchLevelsToCompleteToReachNextSuperLevel(currentLevel);

            //assert
            Assert.AreEqual(expectedQuantity, levels);
        }
        
        [TestCase(0, 4, 9)]
        [TestCase(4, 10, 23)]
        public void HowMuchLevelsToCompleteToReachSuperLevelTest(int currentLevel, int targetSuperLevel, int expectedQuantity)
        {
            //act
            int levels = HowMuchLevelsToCompleteToReachSuperLevel(currentLevel, targetSuperLevel);

            //assert
            Assert.AreEqual(expectedQuantity, levels);
        }
    }
}