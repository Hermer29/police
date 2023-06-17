using UnityEngine;

namespace DefaultNamespace.Audio
{
    public class GlobalAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _loose;
        [SerializeField] private AudioSource _win;
        [SerializeField] private AudioSource _nuke;
        [SerializeField] private AudioSource _cameraMovement;
        [SerializeField] private AudioSource _unitPlacement;
        [SerializeField] private AudioSource _buttons;
        [SerializeField] private AudioSource _activeUnitChange;
        [SerializeField] private AudioSource _upgrade;
        [SerializeField] private AudioSource _backButton;
        
        private void Play(AudioSource audioSource, float f = 0)
        {
            audioSource.Stop();
            if(f == 0)
                audioSource.Play();
            else
                audioSource.PlayDelayed(f);
        }
        
        public void PlayLoose() => Play(_loose);
        public void PlayWin() => Play(_win);
        public void PlayNuke(float f) => Play(_nuke, f);
        public void PlayCameraMovement() => Play(_cameraMovement);
        public void PlayButtonSound() => Play(_buttons);
        public void PlayUnitPlacement() => Play(_unitPlacement);
        public void PlayActiveUnitChange() => Play(_activeUnitChange);
        public void PlayUpgrade() => Play(_upgrade);
        public void PlayBackButton() => Play(_backButton);
    }
}