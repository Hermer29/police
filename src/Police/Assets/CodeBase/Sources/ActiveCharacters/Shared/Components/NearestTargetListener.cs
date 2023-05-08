﻿using System;
using ModestTree;
using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class NearestTargetListener : MonoBehaviour
    {
        [SerializeField] private ColliderReader _reader;

        public event Action<Collider> ClosestObjectLeftTheZone;
        public event Action<Collider> ClosestObjectApproached;

        private Collider _closest;

        private float SqrDistanceToNearest => (_closest.transform.position - transform.position).sqrMagnitude;

        public void FindAgain()
        {
            _closest = null;
            var results = _reader.Overlap();
            foreach (Collider result in results.Except(_closest))
            {
                TriggerEntered(result);
            }
        }
        
        private void Start()
        {
            _reader.TriggerEntered += TriggerEntered;
            _reader.TriggerExit += TriggerExit;
        }

        private void TriggerExit(Collider other)
        {
            if (other.gameObject.activeInHierarchy == false)
                return;
            if (_closest != other)
                return;

            ClosestObjectLeftTheZone?.Invoke(_closest);
        }

        private void TriggerEntered(Collider other)
        {
            if (HaveTargetCloser(other))
                return;

            _closest = other;
            ClosestObjectApproached?.Invoke(other);
        }

        private bool HaveTargetCloser(Collider other)
        {
            return HaveNearest() && IsDistanceMinimalAccordingToLastOne(other) == false;
        }

        private bool HaveNearest()
        {
            return _closest != null;
        }

        private bool IsDistanceMinimalAccordingToLastOne(Collider other)
        {
            return CalculateSqrDistance(other) < SqrDistanceToNearest;
        }

        private float CalculateSqrDistance(Collider other)
        {
            return (other.transform.position - transform.position).sqrMagnitude;
        }
    }
}