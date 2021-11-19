using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Static Data/Player")]
    public class PlayerStaticData: ScriptableObject
    {
        [Range(10,100)]
        public float Health = 100;
        
        [Range(0,1)]
        public float Armor = 0.3f;

        [Range(50,250)]
        public float MoveSpeed = 150;
        
        [Range(50,250)]
        public float RotationSpeed = 150;

        public GameObject Prefab;

        public WeaponStaticData[] AvailableWeapons;
    }
}