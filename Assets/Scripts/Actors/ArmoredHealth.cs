using System;

namespace Actors
{
    public class ArmoredHealth : Health
    {
        public override event Action<float, float> HealthChanged;

        private float _armor;

        public void Initialize(float max, float armor)
        {
            Initialize(max);
            _armor = armor;
        }

        public override void Initialize(float max)
        {
            _max = _current = max;
            HealthChanged?.Invoke(_current, _max);
        }

        public override void ApplyDamage(float damage)
        {
            _current = Math.Max(_current - damage * (1f - _armor), 0f);
            HealthChanged?.Invoke(_current, _max);
        }

        public override void Reset()
        {
            _current = _max;
            HealthChanged?.Invoke(_current, _max);
        }
    }
}