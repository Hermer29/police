using UnityEngine;

namespace MathUtility
{
    public class Functions
    {
        /// <summary>
        /// f(x) = x^n + add
        /// </summary>
        public static float Polynomial(float x, float n, float add = 0) 
            => Mathf.Pow(x, n) + add;
    }
}