using Enemy;
using UnityEngine;

namespace Logic
{
    public abstract class Spawner: MonoBehaviour
    {
        public abstract void Spawn(EnemyDeath enemy);
    }
}