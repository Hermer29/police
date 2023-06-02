using System.Collections;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialEngine : MonoBehaviour
    {
        private ICoroutineRunner _coroutineRunner;
        
        [SerializeField] private TutorialStep[] _steps;
        
        private Coroutine _running;

        [Inject]
        public void Construct(ICoroutineRunner coroutineRunner) 
            => _coroutineRunner = coroutineRunner;

        public void Show()
        {
            Debug.Log($"{nameof(Tutorial)}.{nameof(Show)} called");
            gameObject.SetActive(true);
            _running = _coroutineRunner.StartCoroutine(Running());
        }

        private IEnumerator Running()
        {
            var current = 0;
            while (current < _steps.Length)
            {
                _steps[current].OnEnter.Invoke();
                yield return new WaitWhile(() => _steps[current].StepsEndCondition.IsCompleted == false);
                _steps[current].OnExit.Invoke();
                current++;
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _coroutineRunner.StopCoroutine(_running);
        }
    }
}