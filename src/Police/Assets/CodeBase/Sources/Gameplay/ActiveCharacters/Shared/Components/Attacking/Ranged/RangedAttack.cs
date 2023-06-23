using System.Collections;
using System.Linq;
using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using DefaultNamespace.Audio.Components;
using Gameplay.PeopleDraw.Factory;
using UniRx;
using UnityEngine;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class RangedAttack : Attack
    {
        private BulletFactory _bulletFactory;
        
        [SerializeField] private AimingAnimator _animator;
        [SerializeField] private AttackEffector[] _effector;
        
        public float Cooldown;
        
        private float _remainingCooldown;
        private bool _attackEnabled;
        private bool _attackInProgress;
        private Attackable _target;

        private void Update()
        {
            UpdateCooldown();
            if(CanAttack())
                StartAttack();
        }

        public override void EnableAttack(Attackable target)
        {
            _animator?.StartAiming();
            _attackEnabled = true;
            _target = target;
        }

        private IEnumerator ProcessAttack()
        {
            _attackInProgress = true;
            yield return _effector.Select(effector => effector.Execute(_target).ToObservable())
                    .WhenAll().ToYieldInstruction();

            _attackInProgress = false;
            OnEndAttack();
        }

        public override void DisableAttack()
        {
            _animator?.StopAiming();
            _attackEnabled = false;
        }

        private bool CanAttack() => CooldownIsUp() && _attackEnabled && _attackInProgress == false;

        private bool CooldownIsUp() => _remainingCooldown <= 0;

        private void StartAttack()
        {
            StartCoroutine(ProcessAttack());
                //transform.LookAt(_target.Root);
            OnEndAttack();
        }

        private void UpdateCooldown()
        {
            if(!CooldownIsUp())
                _remainingCooldown -= Time.deltaTime;
        }
        
        private void OnEndAttack() => _remainingCooldown = Cooldown;
    }
}