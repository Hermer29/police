using System;
using UnityEngine;
using Animation = AnimationUtility.Animation;

namespace ActiveCharacters.Shared.Components
{
    public class Attackable : MonoBehaviour
    {
        [field: SerializeField] public Transform Root { get; private set; }
        [SerializeField] private Animation _animation;

        public bool Died;
        public int MaxHp = 1;

        private int _hp;

        public event EventHandler<Attackable> UnitDied;

        private void Start()
        {
            _hp = MaxHp;
        }

        public void ApplyDamage()
        {
            _animation?.Play();
            _hp--;
            if (_hp <= 0)
            {
                Died = true;
                UnitDied?.Invoke(this,this);
            }
        }
    }
}