using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Interface
{
    public class PlayOverTime : MonoBehaviour
    {
        public float Frequency;
        public UnityEvent OnAction;
        public Vector2 Range;
        
        private float StartupTime;
        
        private void OnEnable()
        {
            StartupTime = Random.Range(Range.x, Range.y);
            StartCoroutine(Coroutine());
        }

        private IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(StartupTime);
            while (true)
            {
                OnAction.Invoke();
                yield return new WaitForSeconds(Frequency);
            }
        }
    }
}