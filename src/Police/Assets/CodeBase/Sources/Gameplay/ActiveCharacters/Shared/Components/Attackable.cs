using System;
using DefaultNamespace.Audio;
using DefaultNamespace.Audio.Components;
using Infrastructure;
using UnityEngine;
using Animation = AnimationUtility.Animation;

namespace ActiveCharacters.Shared.Components
{
    public class Attackable : MonoBehaviour
    {
        [field: SerializeField] public Transform Root { get; private set; }
        [SerializeField] private Animation _animation;
        [SerializeField] private DyingAudio _dying;
        
        public bool Died;
        public float MaxHp = 1;

        private float _hp;

        public event EventHandler<Attackable> UnitDied;

        private void Start()
        {
            _hp = MaxHp;
        }

        public void ApplyDamage(float applyingDamage)
        {
            if (applyingDamage <= 0)
                throw new ArgumentOutOfRangeException(nameof(applyingDamage));
            if (Died)
                return;
            AllServices.Get<GlobalAudio>().PlayHitSound();
            Debug.Log($"{nameof(Attackable)}.{nameof(ApplyDamage)} received damage {applyingDamage}. Now: {_hp}, Remains: {_hp - applyingDamage}");
            _animation?.Play();
            _hp -= applyingDamage;
            if (_hp <= 0)
            {
                _dying?.Play();
                Died = true;
                UnitDied?.Invoke(this,this);
            }
        }

        public void Restore()
        {
            Died = false;
            _hp = MaxHp;
        }

        public void Kill()
        {
            ApplyDamage(100000000);
        }

        public void SetHealth(float balanceDtoHealth)
        {
            Died = false;
            MaxHp = balanceDtoHealth;
            _hp = MaxHp;
        }
    }
}