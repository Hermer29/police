using Logic.Audio;
using Services.PrefsService;
using UnityEngine;

namespace Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly IPrefsService _prefsService;

        private const string AudioPrefsKey = "AudioEnabled";

        public AudioService(IPrefsService prefsService)
        {
            _prefsService = prefsService;
            bool state = LoadPersistent();
            SetState(state);
        }

        public bool IsEnabled => LoadPersistent();

        public void ChangeState(bool state)
        {
            SetState(state);
            PersistState(state);
        }

        private void SetState(bool state)
        {
            AudioListener.pause = !state;
            AudioListener.volume = state ? 1 : 0;
        }

        private bool LoadPersistent() => 
            _prefsService.GetInt(AudioPrefsKey, 1) == 1;

        private void PersistState(bool state) => 
            _prefsService.SetInt(AudioPrefsKey, state ? 1 : 0);
    }
}