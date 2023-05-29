using UnityEngine;
using UnityEngine.UI;

namespace Helpers.UI
{
    public class WaitForButtonClick : CustomYieldInstruction
    {
        private bool _clicked;
        private readonly Button _button;

        public WaitForButtonClick(Button button) => (_button = button).onClick.AddListener(OnClicked);
        
        public override bool keepWaiting => _clicked == false;

        private void OnClicked()
        {
            _clicked = true;
            _button.onClick.RemoveListener(OnClicked);
        }
    }
}