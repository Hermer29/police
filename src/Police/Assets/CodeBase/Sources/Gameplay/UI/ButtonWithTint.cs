using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class ButtonWithTint : MonoBehaviour
    {
        public Button Button;
        [SerializeField] private GameObject _tint;
        
        public void ShowTint()
        {
            Button.interactable = false;
            _tint.SetActive(true);
        }
    }
}