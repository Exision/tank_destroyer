using Enemy;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Static Data/Enemy")]
    public class EnemyStaticData: ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        [Range(10,100)]
        public float Health = 10;
        
        [Range(0,1)]
        public float Armor = 0.3f;
        
        [Range(1,30)]
        public float Damage = 10;
        
        [Range(0,250)]
        public float MoveSpeed = 50;

        public GameObject Prefab;
    }
}