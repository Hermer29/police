using System;
using System.Collections;
using Gameplay.ActiveCharacters.Shared.Components;
using Helpers;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LooseTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("_collider")] public BoxCollider Collider;
        public BoxCollider AlmostLostTrigger;
        private bool _triggered;

        public IObservable<Collider> OnAlmostLost => AlmostLostTrigger.OnTriggerEnterAsObservable();

        private void Start()
        {
            Collider = GetComponent<BoxCollider>();
            Collider.isTrigger = true;
        }

        public void Reactivate()
        {
            _triggered = false;
        }

        public IEnumerator WaitForTrigger()
        {
            yield return new WaitUntil(() => _triggered);
            _triggered = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (NotZombieInTrigger(other)) return;
            Debug.Log($"{nameof(LooseTrigger)}.{nameof(OnTriggerEnter)} Zombie detected in loose trigger");
            _triggered = true;
        }

        private static bool NotZombieInTrigger(Collider other)
            => !LayerMaskExtensions.LayerEquals(other, LayerMask.GetMask("Zombie"));
    }
}