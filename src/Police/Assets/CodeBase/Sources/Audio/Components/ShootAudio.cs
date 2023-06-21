using System.Collections;
using ActiveCharacters.Shared.Components;
using Gameplay.ActiveCharacters.Shared.Components.Attacking;
using UnityEngine;

namespace DefaultNamespace.Audio.Components
{
    [RequireComponent(typeof(AudioSource))]
    public class ShootAudio : AttackEffector
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

        public override IEnumerator Execute(Attackable target)
        {
            Play();
            yield break;
        }
    }
}