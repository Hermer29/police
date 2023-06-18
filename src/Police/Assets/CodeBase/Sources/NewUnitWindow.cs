using Helpers;
using Infrastructure.Services.UseService;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Upgrading;
using Upgrading.UnitTypes;
using Zenject;

namespace DefaultNamespace
{
    public class NewUnitWindow : MonoBehaviour, IManuallyInitializable
    {
        private UnitsRepository _repo;

        [SerializeField] private Image _newUnit;
        [SerializeField] private Button _close;
        private bool _initialized;
        private UsedUnitsService _units;

        [Inject]
        public void Construct(UnitsRepository repo, UsedUnitsService units)
        {
            _repo = repo;
            _units = units;
            
            Close();
            _close.onClick.AddListener(Close);
        }

        public void Initialize()
        {
            Subscribe();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void Subscribe()
        {
            foreach (PartialUpgradableUnit partialUpgradableUnit in _repo.GetAll())
            {
                partialUpgradableUnit.CurrentIcon.Skip(1).Subscribe(
                    changed => OnAppearanceChanged(partialUpgradableUnit, changed));
            }

            _units.UsedUnits.ObserveReplace().Subscribe(OnReplace);
        }

        private void OnReplace(DictionaryReplaceEvent<UnitType, PartialUpgradableUnit> upgradableUnit)
        {
            OnAppearanceChanged(upgradableUnit.NewValue, upgradableUnit.NewValue.CurrentIcon.Value);
        }

        private void OnAppearanceChanged(PartialUpgradableUnit partialUpgradableUnit, Sprite icon)
        {
            Debug.Log($"{nameof(NewUnitWindow)}.{partialUpgradableUnit.name} appearance change detected");
            gameObject.SetActive(true);
            _newUnit.sprite = icon;
        }

        public void TryInitialize()
        {
            if (_initialized) return;

            _initialized = true;
            Initialize();
        }
    }
}