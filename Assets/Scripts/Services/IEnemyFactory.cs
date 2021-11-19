using System;
using Enemy;
using StaticData;

namespace Services
{
    public interface IEnemyFactory
    {
        event Action EnemyDied;
        
        void Initialize(EnemyStaticData[] enemyDatas, int maxSize);
        EnemyDeath Get(EnemyTypeId enemyType);
        int GetActiveEnemyCount();
        void CleanUp();
    }
}