using System.Collections;
using ActiveCharacters.Shared.Components;
using Gameplay.PeopleDraw.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class BulletEffector : AttackEffector, IDamageValueDependent
    {
        [SerializeField] private Transform _muzzle;
        
        private BulletFactory _bulletFactory;
        private Attackable _target;
        private bool _endDetected;
        private float _applyingDamage = .1f;

        [Inject]
        public void Construct(BulletFactory bullets)
        {
            _bulletFactory = bullets;
        }

        public void InitializeDamage(float damage)
        {
            _applyingDamage = damage;
        }

        public override IEnumerator Execute(Attackable target)
        {
            _target = target;
            Bullet bullet = _bulletFactory.CreateBullet(_muzzle.position);
            bullet.ShotTowards(target, OnStartAttack);
            yield return new WaitUntil(() => _endDetected);
        }

        private void OnStartAttack()
        {
            _endDetected = true;
            if (_target.Died)
            {
                return;
            }
            _target.ApplyDamage(_applyingDamage);
        }
    }
}