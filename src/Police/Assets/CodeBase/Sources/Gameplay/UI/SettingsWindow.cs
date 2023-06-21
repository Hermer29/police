using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeSettings;
        
        private void Start() 
            => _closeSettings.onClick.AddListener(Hide);

        public void Hide() 
            => gameObject.SetActive(false);

        public void Show()
            => gameObject.SetActive(true);
    }
}