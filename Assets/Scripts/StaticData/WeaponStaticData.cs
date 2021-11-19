using Logic;
using UnityEngine;
using Weapons;

namespace StaticData
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Static Data/Weapon")]
    public class WeaponStaticData: ScriptableObject
    {
        [Range(1,30)]
        public float Damage = 10;

        [Range(0.5f, 5f)]
        public float RateOfFire = 1f;

        public float BulletSpeed;
        
        public Weapon WeaponPrefab; 
        
        public Bullet BulletPrefab;
    }
}