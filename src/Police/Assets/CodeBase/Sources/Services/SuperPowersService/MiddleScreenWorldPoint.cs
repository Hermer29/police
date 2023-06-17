using UnityEngine;

namespace Services.SuperPowersService
{
    public class MiddleScreenWorldPoint
    {
        private RaycastHit? _hit;
    
        private void Initialize()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f));
            int requiredLayerMask = 1 << LayerMask.NameToLayer("RaycastPlane");
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, requiredLayerMask);
            _hit = hit;
        }

        public Vector3 Point
        {
            get
            {
                if(_hit == null)
                    Initialize();
                return _hit.Value.point;
            }
        }

        public Vector3 Normal
        {
            get
            {
                if(_hit == null)
                    Initialize();
                return _hit.Value.normal;
            }
        }
    }
}