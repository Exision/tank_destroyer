using System;
using Actors;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyAttack: MonoBehaviour
    {
        public event Action Attacked;

        private Health _target;
        private float _damage;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Health health))
            {
                if (_target != health)
                    return;

                health.ApplyDamage(_damage);
                Attacked?.Invoke();
            }
        }

        public void SetTarget(Health target) => 
            _target = target;

        public void SetDamage(float damage) => 
            _damage = damage;
    }
}