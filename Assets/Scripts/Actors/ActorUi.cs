using UI;
using UnityEngine;

namespace Actors
{
    public class ActorUi: MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        
        private Health _health;

        private void OnDestroy() => 
            _health.HealthChanged -= UpdateBar;

        public void Initialize(Health health)
        {
            _health = health;
            _health.HealthChanged += UpdateBar;
        }

        private void UpdateBar(float currentHealth, float maxHealth) => 
            _healthBar.SetValue(currentHealth, maxHealth);
    }
}