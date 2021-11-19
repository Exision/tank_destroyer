using System;
using Actors;
using Services;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet: MonoBehaviour, IPoolItem
    {
        public event Action<IPoolItem> Destroyed;

        private Rigidbody2D _rigidbody;
        private float _damage;
        private float _bulletSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = transform.up * _bulletSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Health health))
                health.ApplyDamage(_damage);

            Destroyed?.Invoke(this);
        }

        public void Fire(Vector3 position, Quaternion rotation, float damage, float bulletSpeed)
        {
            _damage = damage;
            _bulletSpeed = bulletSpeed;
            
            transform.SetPositionAndRotation(position, rotation);
        }
    }
}