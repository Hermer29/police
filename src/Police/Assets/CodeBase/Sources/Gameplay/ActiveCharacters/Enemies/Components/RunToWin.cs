using System;
using Gameplay.Levels;
using PeopleDraw;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Components
{
    public class RunToWin : MonoBehaviour
    {
        private LooseTrigger _looseTrigger;
        
        [SerializeField] private NavMeshAgent _agent;

        public void Construct(LooseTrigger looseTrigger)
        {
            _looseTrigger = looseTrigger;
        }
        
        private void Update()
        {
            if(NotInitialized())
                return;
            var pointOnBounds = _looseTrigger.Collider.center + _looseTrigger.transform.position;
            pointOnBounds.y = _agent.transform.position.y;
            var path = new NavMeshPath();
            _agent.isStopped = false;
            var isSuccess = _agent.CalculatePath(pointOnBounds, path);
            _agent.SetPath(path);
        }

        private bool NotInitialized()
        {
            return _looseTrigger == null;
        }

        private Vector3 WinPosition()
        {
            Vector3 position = _agent.transform.position;
            return new Vector3
            {
                x = -8,
                y = position.y,
                z = 1000
            };
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_agent.transform.position, _agent.nextPosition); 
        }
    }
}