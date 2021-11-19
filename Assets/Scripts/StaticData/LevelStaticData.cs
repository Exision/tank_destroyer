using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData: ScriptableObject
    {
        public EnemyStaticData[] LevelEnemies;
        
        public int MaxEnemiesCount;

        public float EnemySpawnDelay;
        
        public Vector3 PlayerStartPoint;
    }
}