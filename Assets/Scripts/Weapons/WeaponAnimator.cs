using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimator: MonoBehaviour
    {
        private readonly int ShootHash = Animator.StringToHash("Shoot");

        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void PlayShoot() => 
            _animator.SetTrigger(ShootHash);
    }
}