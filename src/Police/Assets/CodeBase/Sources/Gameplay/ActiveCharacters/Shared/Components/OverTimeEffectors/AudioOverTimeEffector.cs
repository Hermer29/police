using ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.OverTimeEffectors
{
    public class AudioOverTimeEffector : OverTimeEffector
    {
        [SerializeField] private AudioSource _source;

        public override void StartExecuting(Attackable target)
        {
            _source.loop = true;
            _source.Play();
        }

        public override void EndExecuting()
        {
            _source.Stop();
        }
    }
}