using BalloonBloom.Coloring;
using BalloonBloom.UI;
using TMPro;
using UnityEngine;

namespace BalloonBloom.Core
{
    /// <summary>
    /// Central scene coordinator for lightweight MVP game flow.
    /// </summary>
    public sealed class LevelManager : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private PollenResource pollenResource;
        [SerializeField] private TextMeshProUGUI coloredPercentLabel;

        [Header("Color Progress")]
        [SerializeField] [Range(0.001f, 1f)] private float objectColoredThreshold = 0.95f;

        private int _registeredColorables;
        private int _coloredCompleted;
        private bool _isInitialized;

        public static LevelManager Instance { get; private set; }

        public PollenResource PollenResource => pollenResource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _isInitialized = true;
        }

        private void Start()
        {
            RefreshColoredPercentLabel();
        }

        public void RegisterColorable(ColorableObject colorableObject)
        {
            if (!_isInitialized || colorableObject == null)
            {
                return;
            }

            _registeredColorables++;
            RefreshColoredPercentLabel();
        }

        public void NotifyColorableFilled(ColorableObject colorableObject)
        {
            if (!_isInitialized || colorableObject == null)
            {
                return;
            }

            _coloredCompleted = Mathf.Clamp(_coloredCompleted + 1, 0, _registeredColorables);
            RefreshColoredPercentLabel();
        }

        public float GetColorCompleteThreshold()
        {
            return objectColoredThreshold;
        }

        private void RefreshColoredPercentLabel()
        {
            if (coloredPercentLabel == null)
            {
                return;
            }

            var percent = _registeredColorables <= 0
                ? 0f
                : (_coloredCompleted / (float)_registeredColorables) * 100f;

            coloredPercentLabel.text = $"Color Restored: {percent:0}%";
        }
    }
}
