using System.Collections;
using Logic;
using Services;
using UnityEngine;
using Zenject;

namespace Weapons
{
    [RequireComponent(typeof(WeaponAnimator))]
    public class Weapon: MonoBehaviour
    {
        private const float _bulletOffset = 0.5f;

        private ICoroutineRunner _coroutineRunner;
        private WeaponAnimator _animator;
        private PooledFactory<Bullet> _bulletPool;

        private float _damage;
        private float _bulletSpeed;
        private WaitForSeconds _waitToReload;
        private bool _weaponReady;

        [Inject]
        private void Construct(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        private void Awake()
        {
            _animator = GetComponent<WeaponAnimator>();
        }

        public void Initialize(float damage, float rateOfFire, float bulletSpeed, Bullet bulletPrefab)
        {
            _damage = damage;
            _bulletSpeed = bulletSpeed;
            _waitToReload = new WaitForSeconds(1 / rateOfFire);
            _weaponReady = true;
            
            CreatePool(bulletPrefab);
        }

        public void TryShoot()
        {
            if (!_weaponReady)
                return;
            
            FireBullet();
            ShootAnimation();
            StartReload();
        }

        private void CreatePool(Bullet bulletPrefab)
        {
            _bulletPool = new PooledFactory<Bullet>();
            _bulletPool.Initialize(bulletPrefab);
        }

        private void FireBullet()
        {
            Bullet bullet = _bulletPool.Get();
            
            bullet.Fire(transform.position + transform.up * _bulletOffset, transform.rotation, _damage, _bulletSpeed);
        }

        private void ShootAnimation()
        {
            _animator.PlayShoot();
        }

        private void StartReload()
        {
            _coroutineRunner.StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _weaponReady = false;
            
            yield return _waitToReload;
            
            _weaponReady = true;
        }
    }
}