using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DefaultNamespace.Audio.Components
{
    [RequireComponent(typeof(Button))]
    public class AudioButton : MonoBehaviour
    {
        private GlobalAudio _audio;
        
        [SerializeField] private AudioType _type;

        [Inject]
        public void Construct(GlobalAudio audio)
        {
            _audio = audio;
            GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            switch (_type)
            {
                case AudioType.Back:
                    _audio.PlayBackButton();
                    break;
                case AudioType.Regular:
                    _audio.PlayButtonSound();
                    break;
            }
        }
    }
}