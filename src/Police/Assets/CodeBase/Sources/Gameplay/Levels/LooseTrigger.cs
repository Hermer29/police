using System;
using System.Collections;
using Gameplay.ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LooseTrigger : MonoBehaviour
    {
        private BoxCollider _collider;
        private bool _triggered;

        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
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