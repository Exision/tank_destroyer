using System;
using UnityEngine;
using Zenject;

namespace Services
{
    public interface IInputService: ITickable
    {
        event Action<Vector2> Axis;
        event Action AttackButtonDown;
        event Action NextWeaponButtonDown;
        event Action PreviousWeaponButtonDown;
        event Action AnyKeyDown;
    }
}