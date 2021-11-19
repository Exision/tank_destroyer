using System;
using Services;
using UnityEngine;
using Weapons;
using Zenject;

namespace Hero
{
    public class PlayerAttack: MonoBehaviour
    {
        private IInputService _inputService;

        private Weapon[] _weapons;
        private int _currentWeaponIndex;
        
        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _inputService.AttackButtonDown += Shoot;
            _inputService.NextWeaponButtonDown += SwapToNextWeapon;
            _inputService.PreviousWeaponButtonDown += SwapToPreviousWeapon;
        }

        private void OnDisable()
        {
            _inputService.AttackButtonDown -= Shoot;
            _inputService.NextWeaponButtonDown -= SwapToNextWeapon;
            _inputService.PreviousWeaponButtonDown -= SwapToPreviousWeapon;
        }

        public void Initialize(Weapon[] weapons)
        {
            if (weapons.Length == 0)
                throw new Exception("Need add at least 1 weapon");
            
            _weapons = weapons;
            
            SelectWeapon(0);
        }

        private void Shoot() => 
            _weapons[_currentWeaponIndex].TryShoot();

        private void SwapToNextWeapon()
        {
            int newWeaponIndex = _currentWeaponIndex - 1;

            if (newWeaponIndex < 0)
                newWeaponIndex = _weapons.Length - 1;
            
            if (newWeaponIndex != _currentWeaponIndex)
                SelectWeapon(newWeaponIndex);
        }

        private void SwapToPreviousWeapon()
        {
            int newWeaponIndex = _currentWeaponIndex + 1;

            if (newWeaponIndex >= _weapons.Length)
                newWeaponIndex = 0;
            
            if (newWeaponIndex != _currentWeaponIndex)
                SelectWeapon(newWeaponIndex);
        }

        private void SelectWeapon(int index)
        {
            if (_weapons[_currentWeaponIndex].gameObject.activeSelf)
                _weapons[_currentWeaponIndex].gameObject.SetActive(false);
            
            _currentWeaponIndex = index;
            _weapons[_currentWeaponIndex].gameObject.SetActive(true);
        }
    }
}