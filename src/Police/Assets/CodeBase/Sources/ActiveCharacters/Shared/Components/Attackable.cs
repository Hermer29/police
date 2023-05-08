using System;
using UnityEngine;

namespace ActiveCharacters.Shared.Components
{
    public class Attackable : MonoBehaviour
    {
        [field: SerializeField] public Transform Root { get; private set; }
        
        public bool Died;
        public int MaxHp = 1;

        private int _hp;

        private void Start()
        {
            _hp = MaxHp;
        }

        public void ApplyDamage()
        {
            _hp--;
            if (_hp <= 0)
            {
                Died = true;
            }
        }
    }
}