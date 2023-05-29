using NUnit.Framework;
using UniRx;

namespace CodeBase.Tests
{
    public class UpgradableUnitsTests
    {
        [Test]
        public void WhenLevelPartUpgraded3Times_AndSubscribedOnSuperUpgrade_ThenShouldBeNotification()
        {
            //arrange
            var upgradable = Setup.PartialUpgradeUnit();
            bool triggered = false;
            upgradable.CurrentAppearance.Subscribe(_ => triggered = true);
            
            //act
            upgradable.Upgrade();
            upgradable.Upgrade();
            upgradable.Upgrade();
            
            //assert
            Assert.That(triggered);
        }
        
        [Test]
        public void WhenLevelPartUpgraded6Times_AndSubscribedOnSuperUpgrade_ThenShouldBeTwoNotifications()
        {
            //arrange
            var upgradable = Setup.PartialUpgradeUnit();
            int triggered = 0;
            upgradable.CurrentAppearance.Subscribe(_ => triggered++);
            
            //act
            upgradable.Upgrade();
            upgradable.Upgrade();
            upgradable.Upgrade();
            upgradable.Upgrade();
            upgradable.Upgrade();
            upgradable.Upgrade();
            
            //assert
            Assert.AreEqual(2, triggered);
        }
    }
}