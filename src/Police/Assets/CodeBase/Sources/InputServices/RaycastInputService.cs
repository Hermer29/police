using System;
using UnityEngine;

namespace InputServices
{
    public class RaycastInputService : MonoBehaviour
    {
        private Camera _camera;
        
        private RaycastHit[] _hits = new RaycastHit[1];
        private int TargetLayerMask;
        public event Action<IRaycastDetectableObject> Detected;

        private void Start()
        {
            _camera ??= Camera.main;
            TargetLayerMask = 1 << LayerMask.NameToLayer("RaycastDetectable");
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == false)
                return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (DetectsTheTarget(ray, out IRaycastDetectableObject target))
            {
                Debug.Log($"{nameof(RaycastInputService)}.{nameof(Update)} detected click on object {target.GameObject.name}");
                Detected?.Invoke(target);
            }
        }

        private bool DetectsTheTarget(Ray ray, out IRaycastDetectableObject target)
        {
            target = null;
            return Physics.RaycastNonAlloc(ray, _hits, Mathf.Infinity, TargetLayerMask) != 0 && _hits[0].transform.TryGetComponent(out target);
        }
    }
}