using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

namespace Upgrading.UI.Barracks
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizeString;

        public void SetKey(TableEntryReference entryReference)
        {
            var tableEntry = (TableEntryReference)entryReference.KeyId;
            _localizeString.SetEntry(tableEntry);
        }
    }
}