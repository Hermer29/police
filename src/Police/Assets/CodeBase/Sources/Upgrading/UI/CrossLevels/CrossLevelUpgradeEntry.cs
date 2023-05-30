using System;
using Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.UnitTypes;

namespace Upgrading.UI.CrossLevels
{
    public class CrossLevelUpgradeEntry : MonoBehaviour
    {
        private UpgradableUnit _unit;

        [field: SerializeField] public Button UpgradeForAds { get; private set; }
        [field: SerializeField] public Button UpgradeForCurrency { get; private set; }
        [field: SerializeField] public Button UpgradeForCoins { get; private set; }
        
        [SerializeField] private TMP_Text _upgradeCost;
        [SerializeField] private ProgressBlock _progressBlock;

        public void Construct(UpgradableUnit unit)
        {
            _unit = unit;
        }

        public void ShowOptionToUpgradeForAdvertising()
        {
            
        }

        public void ShowOptionToUpgradeForMoney()
        {
            
        }

        public void ShowUpgradeCost(int cost) 
            => _upgradeCost.text = cost.ToString();

        public void ShowUpgradeLevel(int upgradeLevel) 
            => _progressBlock.ShowLevel(upgradeLevel);
    }
}