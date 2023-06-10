using System;
using Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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

        [SerializeField] private LocalizeStringEvent _localizeString;
        [SerializeField] private TMP_Text _upgradeCost;
        [SerializeField] private ProgressBlock _progressBlock;
        [SerializeField] private GameObject _maxUpgrade;
        [SerializeField] private GameObject _upDoubleArrow;

        public int CurrentLevel { get; private set; }
        
        public void Construct(UpgradableUnit unit)
        {
            _unit = unit;
        }

        public void ShowMaxUpgradeActive(bool state)
        {
            _maxUpgrade.SetActive(state);
            _upDoubleArrow.SetActive(!state);
            UpgradeForAds.gameObject.SetActive(!state);
        }

        public void ShowUpgradeCost(int cost) 
            => _upgradeCost.text = cost.ToString();

        public void ShowUpgradeLevel(int upgradeLevel)
        {
            _progressBlock.ShowLevel(upgradeLevel);
            CurrentLevel = upgradeLevel;
            _localizeString.RefreshString();
        }
    }
}