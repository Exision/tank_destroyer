using System;
using System.Collections.Generic;
using Actors;
using Enemy;
using Hero;
using StaticData;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Services
{
    public class EnemyFactory: IEnemyFactory
    {
        public event Action EnemyDied;
        
        private Dictionary<EnemyTypeId,ObjectPool<EnemyDeath>> _enemyPools;
        private readonly PlayerHealth _player;
        private List<EnemyDeath> _activeEnemies;
        
        public EnemyFactory(PlayerHealth player)
        {
            _player = player;
        }
        
        public void Initialize(EnemyStaticData[] enemyDatas, int maxSize)
        {
            _enemyPools = new Dictionary<EnemyTypeId, ObjectPool<EnemyDeath>>();
            _activeEnemies = new List<EnemyDeath>(maxSize);
            
            FillPool(enemyDatas, maxSize);
        }

        public EnemyDeath Get(EnemyTypeId enemyType)
        {
            EnemyDeath enemy;

            if (_enemyPools.TryGetValue(enemyType, out ObjectPool<EnemyDeath> pool))
                enemy = pool.Get();
            else
                throw new Exception($"Pool for {enemyType} doesn't exist");
            
            _activeEnemies.Add(enemy);
            enemy.GetComponent<Health>().Reset();
            
            return enemy;
        }

        public int GetActiveEnemyCount() => 
            _activeEnemies.Count;

        public void CleanUp()
        {
            _activeEnemies.ForEach(item => Object.Destroy(item.gameObject));
            _activeEnemies.Clear();
            
            foreach (var pool in _enemyPools.Values)
                pool.Clear();
        }

        private void FillPool(EnemyStaticData[] enemyDatas, int maxSize)
        {
            foreach (EnemyStaticData enemyData in enemyDatas)
            {
                var pool = new ObjectPool<EnemyDeath>(
                    () => CreateEnemy(enemyData), 
                    GetEnemy(), 
                    ReleaseEnemy(),
                    DestroyEnemy(),
                    false, 
                    maxSize / 3, 
                    maxSize);

                _enemyPools.Add(enemyData.EnemyTypeId, pool);
            }
        }

        private EnemyDeath CreateEnemy(EnemyStaticData enemyData)
        {
             EnemyDeath enemy = Object.Instantiate(enemyData.Prefab).GetComponent<EnemyDeath>();
             SetData(enemy, enemyData);
             
             return enemy;
        }

        private Action<EnemyDeath> GetEnemy() =>
            item =>
            {
                item.gameObject.SetActive(true);
                item.Died += OnEnemyDied;
            };

        private Action<EnemyDeath> ReleaseEnemy() => 
            item => item.gameObject.SetActive(false);

        private Action<EnemyDeath> DestroyEnemy() =>
            item =>
            {
                Object.Destroy(item.gameObject);
            };

        private void SetData(EnemyDeath enemy, EnemyStaticData enemyData)
        {
            enemy.Initialize(enemyData.EnemyTypeId);

            var enemyAttack = enemy.GetComponent<EnemyAttack>();
            enemyAttack.SetDamage(enemyData.Damage);
            enemyAttack.SetTarget(_player);

            var enemyMove = enemy.GetComponent<EnemyMove>();
            enemyMove.SetMoveSpeed(enemyData.MoveSpeed);
            enemyMove.SetTarget(_player.gameObject);

            ArmoredHealth health = enemy.GetComponent<ArmoredHealth>();
            health.Initialize(enemyData.Health, enemyData.Armor);

            if (enemy.TryGetComponent(out ActorUi actorUi))
                actorUi.Initialize(health);
        }

        private void OnEnemyDied(EnemyTypeId enemyTypeId, EnemyDeath enemy)
        {
            _enemyPools[enemyTypeId].Release(enemy);
            _activeEnemies.Remove(enemy);

            enemy.Died -= OnEnemyDied;

            EnemyDied?.Invoke();
        }
    }
}