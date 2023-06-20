using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

namespace Upgrading.UI.Barracks
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizeString;

        public void SetKey(LocalizedString localizedString)
        {
            _localizeString.StringReference = localizedString;
        }
    }
}