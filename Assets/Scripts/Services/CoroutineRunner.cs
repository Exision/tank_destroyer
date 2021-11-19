using UnityEngine;

namespace Services
{
    public class CoroutineRunner: MonoBehaviour, ICoroutineRunner
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}