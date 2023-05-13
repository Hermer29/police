using System;
using Gameplay.ActiveCharacters.Shared.Components;
using UnityEngine;

namespace Gameplay.Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LooseTrigger : MonoBehaviour
    {
        private BoxCollider _collider;

        public event Action ZombieInTrigger;
        
        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerMaskExtensions.LayerEquals(other, LayerMask.GetMask("Zombie")))
            {
                Debug.Log("Zombie detected in loose trigger");
                ZombieInTrigger?.Invoke();
            }
        }
    }
}