using Interface;
using Logic.Audio;
using UnityEngine;
using Zenject;

namespace Services.Audio
{
    public class AudioServiceController : MonoBehaviour
    {
        private IAudioService _audioService;

        [SerializeField] private ToggleableButton _button;
        
        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
            
            _button.SetState(_audioService.IsEnabled);
            _button.OnToggle.AddListener(OnToggle);
        }

        private void OnToggle(bool state)
        {
            _audioService.ChangeState(state);
        }
    }
}