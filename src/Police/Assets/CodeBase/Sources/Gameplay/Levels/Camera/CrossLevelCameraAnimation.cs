using Cinemachine;
using DefaultNamespace.Audio;
using ModestTree;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels
{
    public class CrossLevelCameraAnimation : MonoBehaviour
    {
        private GlobalAudio _audio;
        
        public CinemachineVirtualCamera[] LevelCameras;

        [Inject]
        public void Construct(GlobalAudio audio)
        {
            _audio = audio;
        }
        
        private int CurrentIndex { get; set; }

        // ReSharper disable once UnusedMember.Global
        public void UseNext()
        {
            if (NoMoreCameras())
            {
                Debug.Log("No more level cameras defined");
                return;
            }
            Initialize(CurrentIndex + 1);
        }

        private bool NoMoreCameras() => LevelCameras.Length - 1 <= CurrentIndex;

        private void Start() => Initialize(cameraIndex: 0);

        private void Initialize(int cameraIndex)
        {
            CurrentIndex = cameraIndex;
            UseCamera(CurrentIndex);
        }

        private void UseCamera(int index)
        {
            _audio.PlayCameraMovement();
            CinemachineVirtualCamera current = LevelCameras[index];
            current.Priority = 11;
            foreach (CinemachineVirtualCamera virtualCamera in LevelCameras.Except(current))
            {
                virtualCamera.Priority = 10;
            }
        }

        public void ShowCameraForLevel(int getLocalLevel)
        {
            Initialize(--getLocalLevel);
        }
    }
}