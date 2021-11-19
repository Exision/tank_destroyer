using UnityEngine;
using UnityEngine.Pool;

namespace Services
{
    public class PooledFactory<TObject> : IFactory<TObject> where TObject : MonoBehaviour, IPoolItem
    {
        private ObjectPool<TObject> _pool;

        public void Initialize(TObject prefab)
        {
            GameObject parent = new GameObject($"{prefab.name}_root");
            
            _pool = new ObjectPool<TObject>(() => Object.Instantiate(prefab, parent.transform), 
                item =>
                {
                    item.Destroyed += OnItemDestroyed;
                    item.gameObject.SetActive(true);
                }, 
                item => item.gameObject.SetActive(false), 
                Object.Destroy, false, 3, 25);
        }

        public TObject Get() => 
            _pool.Get();

        private void OnItemDestroyed(IPoolItem poolItem)
        {
            poolItem.Destroyed -= OnItemDestroyed;
            _pool.Release(poolItem as TObject);
        }
    }
}