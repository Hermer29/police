using System;
using UnityEngine;

namespace DefaultNamespace.Audio.Components
{
    [RequireComponent(typeof(AudioSource))]
    public class DyingAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;

        private void OnValidate()
        {
            _audio = GetComponent<AudioSource>();
            _audio.playOnAwake = false;
        }

        public void Play()
        {
            _audio.Play();
        }
    }
}