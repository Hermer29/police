using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.UseService;
using UI.Factory;
using UnityEngine;
using Upgrading;
using Upgrading.UI.Barracks;
using Upgrading.UnitTypes;
using Zenject;

namespace Barracks
{
    public class BarracksWindow : MonoBehaviour
    {
        private UnitsRepository _unitsRepository;
        private UiElementsFactory _uiElementsFactory;

        [SerializeField] private BarracksUnitGroup[] _groups;
        [SerializeField] private BarracksUpgradeModal _upgradeModal;
        
        private bool _initialized;
        private IEnumerable<PartialUpgradableUnit> _units;

        [Inject]
        public void Construct(UnitsRepository unitsRepository, UiElementsFactory uiElementsFactory, UsedUnitsService service)
        {
            _unitsRepository = unitsRepository;
            _uiElementsFactory = uiElementsFactory;
            foreach (BarracksUnitGroup barracksUnitGroup in _groups) 
                barracksUnitGroup.Construct(_uiElementsFactory, this, service);
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

        public void ShowModal(PartialUpgradableUnit unit) => _upgradeModal.Show(unit);

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