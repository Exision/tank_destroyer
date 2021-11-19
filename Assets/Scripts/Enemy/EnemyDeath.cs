using System;
using Actors;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(EnemyAttack))]
    public class EnemyDeath: MonoBehaviour
    {
        public event Action<EnemyTypeId, EnemyDeath> Died;
        
        private Health _health;
        private EnemyAttack _enemyAttack;

        private EnemyTypeId _enemyTypeId;
        
        private void Awake()
        {
            _health = GetComponent<Health>();
            _enemyAttack = GetComponent<EnemyAttack>();
        }

        private void OnEnable()
        {
            _health.HealthChanged += CheckDeath;
            _enemyAttack.Attacked += OnAttack;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= CheckDeath;
            _enemyAttack.Attacked -= OnAttack;
        }

        public void Initialize(EnemyTypeId typeId)
        {
            _enemyTypeId = typeId;
        }

        private void CheckDeath(float currentHealth, float maxHealth)
        {
            if (currentHealth <= 0f)
                Died?.Invoke(_enemyTypeId, this);
        }

        private void OnAttack()
        {
            Died?.Invoke(_enemyTypeId, this);
        }
    }
}