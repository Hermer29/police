using System;

namespace Logic
{
    [Serializable]
    public class Range
    {
        public float a;
        public float b;

        public bool ValueIsIn(float value)
        {
            return value >= a && value < b;
        }
    }
}