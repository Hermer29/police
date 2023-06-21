using System.Collections;
using ActiveCharacters.Shared.Components;
using Gameplay.PeopleDraw.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.ActiveCharacters.Shared.Components.Attacking
{
    public class BulletEffector : AttackEffector
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private Attacker _attacker;
        
        private BulletFactory _bulletFactory;
        private Attackable _target;
        private bool _endDetected;

        [Inject]
        public void Construct(BulletFactory bullets)
        {
            _bulletFactory = bullets;
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
            _attacker.ApplyDamage(_target);
        }
    }
}