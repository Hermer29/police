using Cinemachine;
using ModestTree;
using UnityEngine;

namespace Gameplay.Levels
{
    public class CrossLevelCameraAnimation : MonoBehaviour
    {
        public CinemachineVirtualCamera[] LevelCameras;

        private int CurrentIndex { get; set; }
        private CinemachineVirtualCamera NextCamera => LevelCameras[CurrentIndex + 1];
        private CinemachineVirtualCamera CurrentCamera => LevelCameras[CurrentIndex];

        // ReSharper disable once UnusedMember.Global
        public void UseNext()
        {
            Initialize(CurrentIndex + 1);
        }

        private void Start()
        {
            Initialize(currentCameraIndex: 0); //TODO: Call externally
        }

        private void Initialize(int currentCameraIndex)
        {
            CurrentIndex = currentCameraIndex;
            UseCamera(CurrentIndex);
        }

        private void UseCamera(int index)
        {
            CinemachineVirtualCamera current = LevelCameras[index];
            current.Priority = 11;
            foreach (CinemachineVirtualCamera virtualCamera in LevelCameras.Except(current))
            {
                virtualCamera.Priority = 10;
            }
        }
    }
}