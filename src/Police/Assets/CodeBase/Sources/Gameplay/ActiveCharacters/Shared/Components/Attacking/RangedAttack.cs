using ActiveCharacters.Shared.Components;
using ActiveCharacters.Shared.Components.Attacking;
using Gameplay.PeopleDraw.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class RangedAttack : Attack
    {
        private BulletFactory _bulletFactory;
        
        [SerializeField] private AimingAnimator _animator;
        [SerializeField] private Transform _muzzle;
        
        public float Cooldown;
        
        private float _remainingCooldown;
        private bool _attackEnabled;
        private Attackable _target;

        [Inject]
        public void Construct(BulletFactory bullets)
        {
            _bulletFactory = bullets;
        }
        
        private void Update()
        {
            UpdateCooldown();
            if(CanAttack())
                StartAttack();
        }

        public override void EnableAttack(Attackable target)
        {
            _animator.StartAiming();
            _attackEnabled = true;
            _target = target;
        }

        public override void DisableAttack()
        {
            _animator.StopAiming();
            _attackEnabled = false;
        }

        private bool CanAttack()
        {
            return CooldownIsUp() && _attackEnabled;
        }

        private bool CooldownIsUp()
        {
            return _remainingCooldown <= 0;
        }

        private void StartAttack()
        {
            transform.LookAt(_target.Root);
            Bullet bullet = _bulletFactory.CreateBullet(_muzzle.position);
            bullet.ShotTowards(_target, OnStartAttack);
            OnEndAttack();
        }

        private void UpdateCooldown()
        {
            if(!CooldownIsUp())
                _remainingCooldown -= Time.deltaTime;
        }

        private void OnStartAttack()
        {
            if (_target.Died)
            {
                _attackEnabled = false;
                return;
            }
            _target.ApplyDamage();
        }

        private void OnEndAttack()
        {
            _remainingCooldown = Cooldown;
        }
    }
}