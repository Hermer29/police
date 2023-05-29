using System.Collections;
using UnityEngine;

namespace Tutorial.StepEndCondition
{
    public class TimePassed : StepsEndCondition
    {
        [SerializeField] private float _time;

        private bool _started;
        private bool _ended;
        
        public override bool IsCompleted
        {
            get
            {
                if (_started == false)
                {
                    StartCoroutine(WaitingCoroutine());
                    _started = true;
                }
                return _ended;
            }
        }

        private IEnumerator WaitingCoroutine()
        {
            yield return new WaitForSeconds(_time);
            _ended = true;
        }
    }
}