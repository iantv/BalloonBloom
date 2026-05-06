using UnityEngine;

namespace BalloonBloom.Coloring
{
    /// <summary>
    /// Soft, short-lived color aura to make pollen restoration feel organic.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PollenImpactAura : MonoBehaviour
    {
        [SerializeField] [Min(0.01f)] private float lifeTime = 0.35f;
        [SerializeField] [Min(0.01f)] private float startScale = 0.4f;
        [SerializeField] [Min(0.01f)] private float endScale = 1.2f;
        [SerializeField] [Range(0f, 1f)] private float startAlpha = 0.35f;
        [SerializeField] private Color auraColor = new Color(1f, 0.93f, 0.62f, 1f);

        private SpriteRenderer _renderer;
        private float _elapsed;
        private float _scaleMultiplier = 1f;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = new Color(auraColor.r, auraColor.g, auraColor.b, startAlpha);
            transform.localScale = Vector3.one * startScale;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(_elapsed / lifeTime);

            var alpha = Mathf.Lerp(startAlpha, 0f, t);
            _renderer.color = new Color(auraColor.r, auraColor.g, auraColor.b, alpha);

            var currentScale = Mathf.Lerp(startScale, endScale, t) * _scaleMultiplier;
            transform.localScale = Vector3.one * currentScale;

            if (_elapsed >= lifeTime)
            {
                Destroy(gameObject);
            }
        }

        public void Configure(float scaleMultiplier)
        {
            _scaleMultiplier = Mathf.Max(0.1f, scaleMultiplier);
        }
    }
}
