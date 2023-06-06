using System.Collections.Generic;
using UI.Factory;
using UnityEngine;
using Upgrading.UnitTypes;

namespace Barracks
{
    public class BarracksUnitGroup : MonoBehaviour
    {
        private UiElementsFactory _factory;

        public int CurrentLevel = 0;
        
        [SerializeField] private RectTransform _parent;
        [field: SerializeField] public UnitType UnitType { get; private set; }

        public void Construct(UiElementsFactory factory) => _factory = factory;

        public void BuildUiGroupUnitsByType(IEnumerable<PartialUpgradableUnit> units)
        {
            var group = units
                .OrderByLogic();
            
            foreach (PartialUpgradableUnit unit in group) CreateUnitUiElement(unit);
        }

        private void CreateUnitUiElement(PartialUpgradableUnit unit)
            => _factory.CreateBarracksElement(_parent).Construct(unit);
    }
}