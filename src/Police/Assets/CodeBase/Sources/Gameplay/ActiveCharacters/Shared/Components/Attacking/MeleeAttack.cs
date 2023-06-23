using DefaultNamespace.Audio.Components;
using Gameplay.ActiveCharacters.Shared.Components;
using UnityEngine;

namespace ActiveCharacters.Shared.Components.Attacking
{
    public class MeleeAttack : Attack
    {
        [SerializeField] private AttackingAnimator _animator;
        [SerializeField] private AttackAnimationEventsListener _eventsListener;
        [SerializeField] private ShootAudio _shoot;
        [SerializeField] private Attacker _attacker;
        
        public float Cooldown;
        
        private float _remainingCooldown;
        private bool _isAttacking;
        private bool _attackEnabled;
        private Attackable _target;

        private void Start()
        {
            _eventsListener.AttackStarted += OnStartAttack;
            _eventsListener.AttackEnded += OnEndAttack;
        }

        private void Update()
        {
            UpdateCooldown();
            if(CanAttack())
                StartAttack();
        }

        public override void EnableAttack(Attackable target) => 
            (_attackEnabled, _target) = (true, target);

        public override void DisableAttack() => 
            _attackEnabled = false;

        private bool CanAttack()
        {
            return CooldownIsUp() && !_isAttacking && _attackEnabled;
        }

        private bool CooldownIsUp()
        {
            return _remainingCooldown <= 0;
        }

        private void StartAttack()
        {
            _isAttacking = true;
            _shoot?.Play();
            transform.LookAt(_target.Root); 
            _animator.Attack();
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
            _attacker.ApplyDamage(_target);
        }

        private void OnEndAttack()
        {
            _remainingCooldown = Cooldown;
            _isAttacking = false;
        }
    }
}