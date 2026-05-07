using BalloonBloom.Coloring;
using BalloonBloom.UI;
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

        [Header("Color Progress")]
        [SerializeField] [Range(0.001f, 1f)] private float objectColoredThreshold = 0.95f;

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
        }

        public void RegisterColorable(ColorableObject colorableObject)
        {
            if (!_isInitialized || colorableObject == null)
            {
                return;
            }

        }

        public void NotifyColorableFilled(ColorableObject colorableObject)
        {
            if (!_isInitialized || colorableObject == null)
            {
                return;
            }

        }

        public float GetColorCompleteThreshold()
        {
            return objectColoredThreshold;
        }

    }
}
