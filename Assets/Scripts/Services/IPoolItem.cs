using System;

namespace Services
{
    public interface IPoolItem
    {
        event Action<IPoolItem> Destroyed;
    }
}