using System;
using Interface;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Upgrading.UnitTypes;
using Zenject;

namespace Barracks
{
    public class BarracksUnitElement : MonoBehaviour
    {
        [SerializeField] private ProgressBlock _block;
        [SerializeField] private Image _icon;
        [field:SerializeField] public Button UpgradeWindowOpening { get; private set; }

        private PartialUpgradableUnit _unit;

        public void Construct(PartialUpgradableUnit unit)
        {
            unit.UpgradedLevel
                .Subscribe(OnLevelUpdated)
                .AddTo(this);

            _icon.sprite = unit.CurrentAppearance.Value.Illustration;
            unit.CurrentAppearance.Subscribe(newVal =>
            {
                _icon.sprite = newVal.Illustration;
            });
            _unit = unit;
        }

        private void OnLevelUpdated(int level) => _block.ShowLevel(level);
    }
}