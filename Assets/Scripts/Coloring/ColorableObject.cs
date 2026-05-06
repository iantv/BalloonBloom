using BalloonBloom.Core;
using UnityEngine;

namespace BalloonBloom.Coloring
{
    /// <summary>
    /// Represents a grayscale-to-color transition target.
    /// </summary>
    public sealed class ColorableObject : MonoBehaviour
    {
        [Header("Renderers")]
        [SerializeField] private SpriteRenderer blackWhiteRenderer;
        [SerializeField] private SpriteRenderer colorRenderer;

        [Header("Reveal")]
        [SerializeField] [Range(0.01f, 2f)] private float alphaGainPerHit = 0.08f;
        [SerializeField] [Range(0.1f, 20f)] private float smoothSpeed = 7f;

        private float _targetAlpha;
        private bool _isCompleted;

        private void Awake()
        {
            if (colorRenderer != null)
            {
                var color = colorRenderer.color;
                color.a = 0f;
                colorRenderer.color = color;
            }
        }

        private void Start()
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.RegisterColorable(this);
            }
        }

        private void Update()
        {
            if (colorRenderer == null)
            {
                return;
            }

            var current = colorRenderer.color;
            var nextAlpha = Mathf.Lerp(current.a, _targetAlpha, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
            current.a = nextAlpha;
            colorRenderer.color = current;

            if (_isCompleted || LevelManager.Instance == null)
            {
                return;
            }

            if (current.a >= LevelManager.Instance.GetColorCompleteThreshold())
            {
                _isCompleted = true;
                LevelManager.Instance.NotifyColorableFilled(this);
            }
        }

        public void ApplyPollenHit(float intensity = 1f)
        {
            Debug.Log($"ApplyPollenHit on {name}, targetAlpha={_targetAlpha}");
            if (colorRenderer == null)
            {
                return;
            }

            _targetAlpha = Mathf.Clamp01(_targetAlpha + alphaGainPerHit * Mathf.Max(0.01f, intensity));
        }

        private void Reset()
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            if (renderers.Length >= 2)
            {
                blackWhiteRenderer = renderers[0];
                colorRenderer = renderers[1];
            }
        }
    }
}
