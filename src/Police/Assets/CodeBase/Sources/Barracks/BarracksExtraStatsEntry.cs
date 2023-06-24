using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Barracks
{
    public class BarracksExtraStatsEntry : MonoBehaviour
    {
        [SerializeField] private Image _stat;
        [SerializeField] private TMP_Text _quantity;
        [SerializeField] private LocalizeStringEvent _text;
        
        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
        public void Set(Sprite sprite, float percent, LocalizedString localizedString)
        {
            gameObject.SetActive(true);

            _text.StringReference = localizedString;
            _stat.sprite = sprite;
            _quantity.text = $"+{percent * 100}%";
        }
    }
}