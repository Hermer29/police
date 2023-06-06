using System;
using System.Collections.Generic;
using UI.Factory;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using Upgrading.UnitTypes;

namespace Barracks
{
    public class BarracksUnitGroup : MonoBehaviour
    {
        private UiElementsFactory _factory;
        public LocalizeStringEvent StringEvent;
        private IEnumerable<PartialUpgradableUnit> _group;
        
        [SerializeField] private RectTransform _parent;
        [field: SerializeField] public UnitType UnitType { get; private set; }

        private PartialUpgradableUnit _current;
        [Tooltip("Used by localization")]public int CurrentLevel;
        private IDisposable _previousSubscription;

        public void Construct(UiElementsFactory factory) => _factory = factory;

        public void BuildUiGroupUnitsByType(IEnumerable<PartialUpgradableUnit> units)
        {
            var group = units
                .OrderByLogic();
            _group = group;
            foreach (PartialUpgradableUnit unit in group) CreateUnitUiElement(unit);

            _current = group.GetCurrentFromOrdered();
            ApplyCurrentUnit();
        }

        private void ApplyCurrentUnit()
        {
            SubscribeOnLevelUpdate();
            UpdateLocalizedCurrentLevel();
        }

        private void SubscribeOnLevelUpdate()
        {
            _previousSubscription?.Dispose();
            _previousSubscription = _current.UpgradedLevel.Subscribe(_ => OnUpgrade());
        }

        private void UpdateLocalizedCurrentLevel()
        {
            CurrentLevel = _current.UpgradedLevel.Value;
            StringEvent.RefreshString();
        }

        private void OnUpgrade()
        {
            if (!_current.IsFullyUpgraded())
            {
                UpdateLocalizedCurrentLevel();
                return;
            }

            SelectNextOrIgnore();
        }

        private void SelectNextOrIgnore()
        {
            PartialUpgradableUnit current = _group.GetNext(_current);
            if (current == null)
                return;
            _current = current;
            ApplyCurrentUnit();
        }

        private void CreateUnitUiElement(PartialUpgradableUnit unit)
            => _factory.CreateBarracksElement(_parent).Construct(unit);
    }
}