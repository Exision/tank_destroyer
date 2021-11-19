using Enemy;
using Services;
using UnityEngine;
using Zenject;

namespace Logic
{
    public class EnemySpawner: Spawner
    {
        [SerializeField] private BoxCollider2D[] _spawnZones;
        
        private IRandomService _randomService;

        [Inject]
        private void Construct(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public override void Spawn(EnemyDeath enemy) => 
            enemy.gameObject.transform.SetPositionAndRotation(GetSpawnPosition(),Quaternion.identity);

        private  Vector3 GetSpawnPosition()
        {
            int boxIndex = _randomService.Next(0, _spawnZones.Length);

            return GetRandomPointInsideCollider(_spawnZones[boxIndex]);
        }

        private Vector2 GetRandomPointInsideCollider(BoxCollider2D boxCollider)
        {
            Vector2 extents = boxCollider.size / 2f;
            Vector2 point = new Vector2(_randomService.Next(-extents.x, extents.x), _randomService.Next(-extents.y, extents.y)) + boxCollider.offset;
            
            return boxCollider.transform.TransformPoint(point);
        }
    }
}