using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image _healthImage;

        public void SetValue(float current, float max) => 
            _healthImage.fillAmount = current / max;
    }
}