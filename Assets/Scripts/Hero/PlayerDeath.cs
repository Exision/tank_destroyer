using System;
using Actors;
using UnityEngine;

namespace Hero
{
    [RequireComponent(typeof(Health))]
    public class PlayerDeath: MonoBehaviour
    {
        public event Action Died;
        
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable() => 
            _health.HealthChanged += CheckDeath;

        private void OnDisable() => 
            _health.HealthChanged -= CheckDeath;

        private void CheckDeath(float currentHealth, float maxHealth)
        {
            if (currentHealth <= 0f)
                Died?.Invoke();
        }
    }
}