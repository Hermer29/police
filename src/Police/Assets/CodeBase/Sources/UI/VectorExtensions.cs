using UnityEngine;

namespace UI
{
    public static class VectorExtensions
    {
        public static Vector3 SetZ(this Vector3 source, float z)
        {
            source.z = z;
            return source;
        }
    }
}