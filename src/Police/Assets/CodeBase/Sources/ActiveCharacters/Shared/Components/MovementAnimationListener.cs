using System;
using UnityEngine;
using UnityEngine.AI;

namespace ActiveCharacters.Shared.Components
{
    public class MovementAnimationListener : MonoBehaviour
    {
        private const float MinimalVelocity = 0.1f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private MovementAnimator _animator;
        
        private void Update()
        {
            _animator.SetMovementEnabled(ShouldMove());
        }

        private bool ShouldMove()
        {
            return _agent.velocity.magnitude > MinimalVelocity && _agent.remainingDistance > _agent.radius;
        }
    }
}