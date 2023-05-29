using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UI.Factory;
using UnityEngine;
using Upgrading;
using Upgrading.UnitTypes;
using Zenject;

namespace Barracks
{
    public class BarracksWindow : MonoBehaviour
    {
        private UnitsRepository _unitsRepository;
        private UiElementsFactory _uiElementsFactory;
        
        [SerializeField] private RectTransform _parentForBarriers;
        [SerializeField] private RectTransform _parentForMelee;
        [SerializeField] private RectTransform _parentForRanged;

        private bool _initialized;
        private IEnumerable<PartialUpgradableUnit> _units;

        [Inject]
        public void Construct(UnitsRepository unitsRepository, UiElementsFactory uiElementsFactory)
        {
            _unitsRepository = unitsRepository;
            _uiElementsFactory = uiElementsFactory;
        }
        
        public void Show()
        {
            if (_initialized == false)
            {
                Initialize();
                _initialized = true;
            }
            
            gameObject.SetActive(true);
        }

        public void Hide() 
            => gameObject.SetActive(false);

        private void Initialize()
        {
            _units = _unitsRepository.GetAll();
            var partialUpgradableUnits = _units as PartialUpgradableUnit[] ?? _units.ToArray();
            BuildUiGroupUnitsByType(partialUpgradableUnits, UnitType.Barrier);
            BuildUiGroupUnitsByType(partialUpgradableUnits, UnitType.Melee);
            BuildUiGroupUnitsByType(partialUpgradableUnits, UnitType.Ranged);
        }

        private void BuildUiGroupUnitsByType(IEnumerable<PartialUpgradableUnit> units, UnitType type)
        {
            var group = units
                .Where(x => x.Type == type)
                .OrderByLogic();
            
            foreach (PartialUpgradableUnit unit in group)
            {
                CreateUnitEntry(unit);
            }
        }

        private void CreateUnitEntry(PartialUpgradableUnit unit)
        {
            RectTransform parent = unit.Type switch
            {
                UnitType.Barrier => _parentForBarriers,
                UnitType.Melee => _parentForMelee,
                UnitType.Ranged => _parentForRanged,
                _ => throw new ArgumentOutOfRangeException()
            };
            CreateUnitUiElement(unit, parent);
        }

        private void CreateUnitUiElement(PartialUpgradableUnit unit, Transform parent) 
            => _uiElementsFactory.CreateBarracksElement(parent).Construct(unit);
    }
}