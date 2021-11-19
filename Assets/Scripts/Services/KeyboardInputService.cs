using System;
using UnityEngine;

namespace Services
{
    public class KeyboardInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const KeyCode AttackKeyCode = KeyCode.X;
        private const KeyCode NextWeaponKeyCode = KeyCode.W;
        private const KeyCode PreviousWeaponKeyCode = KeyCode.Q;

        public event Action<Vector2> Axis;
        public event Action AttackButtonDown;
        public event Action NextWeaponButtonDown;
        public event Action PreviousWeaponButtonDown;
        public event Action AnyKeyDown;

        public void Tick()
        {
            UpdateAxis();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (Input.anyKeyDown)
                AnyKeyDown?.Invoke();
            
            if (!Input.anyKey)
                return;

            if (Input.GetKey(AttackKeyCode))
                AttackButtonDown?.Invoke();

            if (Input.GetKeyDown(NextWeaponKeyCode))
                NextWeaponButtonDown?.Invoke();
            
            if (Input.GetKeyDown(PreviousWeaponKeyCode))
                PreviousWeaponButtonDown?.Invoke();
        }

        private void UpdateAxis() => 
            Axis?.Invoke(new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)));
    }
}