using System;
using Interface;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.UnitTypes;

namespace Barracks
{
    public class BarracksUnitElement : MonoBehaviour
    {
        [SerializeField] private ProgressBlock _block;
        [SerializeField] private Button _upgradeWindowOpening;
        
        private PartialUpgradableUnit _unit;

        public void Construct(PartialUpgradableUnit unit)
        {
            unit.UpgradedLevel
                .Subscribe(OnLevelUpdated)
                .AddTo(this);

            _unit = unit;
            _upgradeWindowOpening.onClick.AddListener(OnOpeningWindow);
        }

        public event Action<PartialUpgradableUnit> OpenWindowRequested;

        private void OnOpeningWindow()
        {
            OpenWindowRequested?.Invoke(_unit);
        }

        private void OnLevelUpdated(int level)
        {
            _block.ShowLevel(level);
        }
    }
}