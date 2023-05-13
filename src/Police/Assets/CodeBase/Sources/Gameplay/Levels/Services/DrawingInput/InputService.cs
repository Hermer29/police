using System;
using UnityEngine;

namespace Services
{
    public class InputService : MonoBehaviour, IInputService
    {
        private Camera _camera;
        
        [SerializeField] private LayerMask _drawingPlane;

        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private RaycastHit? _previous;

        public event Action<RaycastHit, RaycastHit> DrawnAtPoint;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void DragInProgress()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            int hits = Physics.RaycastNonAlloc(ray, _hits, Mathf.Infinity, _drawingPlane);
            if (hits == 0)
                return;
            RaycastHit hit = _hits[0];
            DrawnAtPoint?.Invoke(hit, _previous ?? hit);
            _previous = hit;
        }
    }
}