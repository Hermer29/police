using System;
using UnityEngine;

namespace Helpers
{
    public class ColliderListener : MonoBehaviour
    {
        public Action Listener;

        private void OnCollisionEnter(Collision collision)
        {
            Listener?.Invoke();
        }
    }
}