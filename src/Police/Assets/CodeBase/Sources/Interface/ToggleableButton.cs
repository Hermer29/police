using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interface
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class ToggleableButton : MonoBehaviour
    {
        [SerializeField] private Sprite _enabledState;
        [SerializeField] private Sprite _disabledState;
        [SerializeField] private Image _image;

        private Button _button;
        private bool _state = true;
        
        public UnityEvent<bool> OnToggle;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ToggleState);
            SetState(_state);
        }

        public void SetState(bool state)
        {
            _state = state;
            _image.sprite = state ? _enabledState : _disabledState;
        }

        private void ToggleState()
        {
            SetState(!_state);
            OnToggle.Invoke(_state);
        }
    }
}