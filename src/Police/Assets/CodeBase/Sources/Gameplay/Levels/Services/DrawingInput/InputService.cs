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
        private bool _disabled;

        public event Action<RaycastHit> DrawnAtPoint;
        public event Action<RaycastHit> DragInProgress;
        public event Action DragEnded;
        public event Action<Vector3> DragStarted;
        public void Disable() => _disabled = true;

        public void Enable() => _disabled = false;

        private void Awake() => _camera = Camera.main;

        public void OnDrawnAtPoint()
        {
            if (_disabled)
                return;
            
            if (TryGetHit(out RaycastHit hit))
            {
                DrawnAtPoint?.Invoke(hit);
            }
        }

        public void OnDragStarted()
        {
            if (_disabled)
                return;
            
            if (TryGetHit(out RaycastHit hit))
            {
                DragStarted?.Invoke(hit.point);
            }
        }

        public void OnDragEnded()
        {
            if (_disabled)
                return;
            
            DragEnded?.Invoke();
        }
        
        public void OnDragInProgress()
        {
            if (_disabled)
                return;
                
            if (TryGetHit(out var hit))
            {
                DragInProgress?.Invoke(hit);
            }
        }

        private bool TryGetHit(out RaycastHit hit)
        {
            hit = default;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            int raycastDetectable = 1 << LayerMask.NameToLayer("RaycastDetectable");
            int count = Physics.RaycastNonAlloc(
                ray: ray, _hits, Mathf.Infinity, _drawingPlane | raycastDetectable);
            if (count == 0)
                return false;
            hit = _hits[0];
            return 1 << hit.transform.gameObject.layer != raycastDetectable;
        }
    }
}