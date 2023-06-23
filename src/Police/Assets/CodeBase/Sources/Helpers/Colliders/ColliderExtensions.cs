using System;
using UI;
using UnityEngine;

namespace Helpers
{
    public static class ColliderExtensions
    {
        public static Collider GetNearest(this Transform transform, Collider[] colliders)
        {
            var nearest = (Collider)null;
            var smallestDistance = float.MaxValue;
            
            foreach (Collider collider in colliders)
            {
                if(collider == null)
                    continue;
                var magnitude = Vector3.SqrMagnitude(collider.transform.position - transform.position);
                if (magnitude < smallestDistance)
                {
                    smallestDistance = magnitude;
                    nearest = collider;
                }
            }
            return nearest;
        }

        public static RaycastHit GetNearest(this Transform transform, RaycastHit[] hits)
        {
            var nearest = (RaycastHit)default;
            var smallestDistance = float.MaxValue;
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;
                var magnitude = Vector3.SqrMagnitude(hit.transform.position.SetY(0) - transform.position.SetY(0));
                if (magnitude < smallestDistance)
                {
                    smallestDistance = magnitude;
                    nearest = hit;
                }
            }
            return nearest;
        }

        public static Action OnCollisionEnter(this Collider collider)
        {
            if (collider.TryGetComponent<ColliderListener>(out var colliderListener))
            {
                return colliderListener.Listener;
            }
            return collider.gameObject.AddComponent<ColliderListener>().Listener;
        }
    }
}