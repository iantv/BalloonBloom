using UnityEngine;
using UnityEngine.UI;

namespace BalloonBloom.UI
{
    /// <summary>
    /// Stores and visualizes available pollen resource.
    /// </summary>
    public sealed class PollenResource : MonoBehaviour
    {
        [Header("Resource")]
        [SerializeField] [Min(1f)] private float maxPollen = 100f;
        [SerializeField] [Min(0f)] private float rechargePerSecond = 10f;
        [SerializeField] private bool autoRecharge = true;

        [Header("UI")]
        [SerializeField] private Slider pollenSlider;

        private float _currentPollen;

        public float Normalized => maxPollen <= 0f ? 0f : _currentPollen / maxPollen;
        public float Current => _currentPollen;

        private void Awake()
        {
            _currentPollen = maxPollen;
            UpdateSlider();
        }

        private void Update()
        {
            if (!autoRecharge || _currentPollen >= maxPollen)
            {
                return;
            }

            Add(rechargePerSecond * Time.deltaTime);
        }

        public bool Consume(float amount)
        {
            if (amount <= 0f)
            {
                return true;
            }

            if (_currentPollen <= 0f)
            {
                return false;
            }

            _currentPollen = Mathf.Max(0f, _currentPollen - amount);
            UpdateSlider();
            return _currentPollen > 0f;
        }

        public void Add(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            _currentPollen = Mathf.Min(maxPollen, _currentPollen + amount);
            UpdateSlider();
        }

        private void UpdateSlider()
        {
            if (pollenSlider == null)
            {
                return;
            }

            pollenSlider.minValue = 0f;
            pollenSlider.maxValue = maxPollen;
            pollenSlider.value = _currentPollen;
        }
    }
}
