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

        public static Vector3 SetY(this Vector3 source, float y)
        {
            source.y = y;
            return source;
        }

        public static Vector3 Clamp(this Vector3 source, Vector3 min, Vector3 max)
        {
            return new Vector3
            {
                x = Mathf.Clamp(min.x, max.x, source.x),
                y = Mathf.Clamp(min.y, max.y, source.y),
                z = Mathf.Clamp(min.z, max.z, source.z)
            };
        }

        public static Vector3 AddY(this Vector3 source, float value)
        {
            source.y += value;
            return source;
        }
    }
}