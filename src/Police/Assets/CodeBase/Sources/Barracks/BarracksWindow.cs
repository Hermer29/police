using System.Collections.Generic;
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

        [SerializeField] private BarracksUnitGroup[] _groups;

        private bool _initialized;
        private IEnumerable<PartialUpgradableUnit> _units;

        [Inject]
        public void Construct(UnitsRepository unitsRepository, UiElementsFactory uiElementsFactory)
        {
            _unitsRepository = unitsRepository;
            _uiElementsFactory = uiElementsFactory;
            foreach (BarracksUnitGroup barracksUnitGroup in _groups) 
                barracksUnitGroup.Construct(_uiElementsFactory);
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
            foreach (BarracksUnitGroup barracksUnitGroup in _groups)
            {
                barracksUnitGroup.BuildUiGroupUnitsByType(partialUpgradableUnits.Where(
                    x => x.Type == barracksUnitGroup.UnitType));
            }
        }
    }
}