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
        [field:SerializeField] public Button UpgradeWindowOpening { get; private set; }
        
        private PartialUpgradableUnit _unit;

        public void Construct(PartialUpgradableUnit unit)
        {
            unit.UpgradedLevel
                .Subscribe(OnLevelUpdated)
                .AddTo(this);

            _unit = unit;
        }

        private void OnLevelUpdated(int level) => _block.ShowLevel(level);
    }
}