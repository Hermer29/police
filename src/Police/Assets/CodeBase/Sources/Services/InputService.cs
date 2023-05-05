using System;
using System.Linq;
using UnityEngine;

namespace Services
{
    public class InputService : MonoBehaviour, IInputService
    {
        [SerializeField] private LayerMask _drawingPlane;

        private Camera _camera;
        
        public event Action<RaycastHit> DrawnAtPoint;

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0) == false)
                return;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            RaycastHit[] results = new RaycastHit[1];
            Physics.RaycastNonAlloc(ray, results, Mathf.Infinity, _drawingPlane);
            DrawnAtPoint?.Invoke(results[0]);
        }
    }
}