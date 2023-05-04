using System;
using UnityEngine;

namespace Services
{
    public class DrawingService : MonoBehaviour
    {
        [SerializeField] private LayerMask _drawingPlane;
        [SerializeField] private LayerMask _blocking;

        private Camera _camera;
        
        public event Action Drawn;

        private void Awake()
        {
            
        }
        
        private void Update()
        {
            
        }
    }
}