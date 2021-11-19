using System;
using UnityEngine;

namespace Actors
{
    public abstract class Health: MonoBehaviour
    {
        public abstract event Action<float, float> HealthChanged;
        
        protected float _max;
        protected float _current;
        
        public abstract void Initialize(float max);
        public abstract void ApplyDamage(float damage);
        public abstract void Reset();
    }
}