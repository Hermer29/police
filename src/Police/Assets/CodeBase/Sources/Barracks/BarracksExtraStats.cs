using DefaultNamespace.Gameplay.ActiveCharacters.Stats;
using UnityEngine;
using UnityEngine.Localization;
using Upgrading.UnitTypes;

namespace Barracks
{
    public class BarracksExtraStats : MonoBehaviour
    {
        [SerializeField] private GameObject _noBonuses;
        [SerializeField] private BarracksExtraStatsEntry _objectOne;
        [SerializeField] private BarracksExtraStatsEntry _objectTwo;
        [SerializeField] private Sprite _damageSprite;
        [SerializeField] private Sprite _healthSprite;
        [SerializeField] private Sprite _rangeSprite;
        [SerializeField] private LocalizedString _rangeLocalizedName;
        [SerializeField] private LocalizedString _damageLocalizedName;
        [SerializeField] private LocalizedString _healthLocalizedName;

        public void Show(MeleeTableEntry meleeTableEntry)
        {
            DisableBoth();
            var isShownFirst = TryShowEntry(_damageSprite, meleeTableEntry.ExtraDamageModifier, _objectOne, _damageLocalizedName);
            var isShownSecond = TryShowEntry(_healthSprite, meleeTableEntry.ExtraHealthModifier, _objectTwo, _healthLocalizedName);
            _noBonuses.SetActive(!(isShownFirst && isShownSecond));
        }

        private bool TryShowEntry(Sprite damageSprite, float extraDamageModifier, 
            BarracksExtraStatsEntry barracksExtraStatsEntry, LocalizedString localizedStatName)
        {
            if (extraDamageModifier == 0)
                return false;
            barracksExtraStatsEntry.Set(damageSprite, extraDamageModifier, localizedStatName);
            return true;
        }

        public void Show(RangedStatsEntry entry)
        {
            DisableBoth();
            var isShownFirst = TryShowEntry(_rangeSprite, entry.ExtraRangeModifier, _objectOne, _rangeLocalizedName);
            var isShownSecond = TryShowEntry(_healthSprite, entry.ExtraHealthModifier, _objectTwo, _healthLocalizedName);
            _noBonuses.SetActive(!(isShownFirst && isShownSecond));
        }

        public void Show(BarriersStatsEntry entry)
        {
            DisableBoth();
            var isShownFirst = TryShowEntry(_healthSprite, entry.ExtraHealthModifier, _objectTwo, _healthLocalizedName);
            _noBonuses.SetActive(!isShownFirst);
        }

        private void DisableBoth()
        {
            _objectOne.gameObject.SetActive(false);
            _objectTwo.gameObject.SetActive(false);
        }
    }
}