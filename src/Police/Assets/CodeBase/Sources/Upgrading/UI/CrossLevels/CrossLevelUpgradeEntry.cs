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

        [SerializeField] private Button Upgrade;
        [SerializeField] private TMP_Text _upgradeCost;
        [SerializeField] private ProgressBlock _progressBlock;

        public event Action<UpgradableUnit> OnUpgradingForMoney;
        public event Action<UpgradableUnit> OnUpgradingForRealCurrency;
        public event Action<UpgradableUnit> OnUpgradingForAdvertising;

        public void Construct(UpgradableUnit unit)
        {
            _unit = unit;
            Upgrade.onClick.AddListener(OnClick);
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

        private void OnClick() 
            => OnUpgradingForMoney?.Invoke(_unit);
    }
}