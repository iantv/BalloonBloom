using UnityEngine;

namespace BalloonBloom.Coloring
{
    /// <summary>
    /// Simple gameplay droplet that paints colorable targets.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public sealed class PollenParticle : MonoBehaviour
    {
        [SerializeField] [Range(0.05f, 2f)] private float hitIntensity = 0.2f;
        [SerializeField] [Min(0.1f)] private float lifeTime = 2.5f;
        [SerializeField] private PollenImpactAura auraPrefab;
        [SerializeField] [Range(0.25f, 2f)] private float auraScaleMultiplier = 1f;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifeTime);
        }

        public void Initialize(Vector2 velocity)
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }

            _rigidbody2D.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var colorable = ResolveColorableTarget(other);
            if (colorable != null)
            {
                colorable.ApplyPollenHit(hitIntensity);
                SpawnAura(other);
            }

            Destroy(gameObject);
        }

        private ColorableObject ResolveColorableTarget(Collider2D other)
        {
            if (other == null)
            {
                return null;
            }

            var onSelf = other.GetComponent<ColorableObject>();
            if (onSelf != null)
            {
                return onSelf;
            }

            var inParent = other.GetComponentInParent<ColorableObject>();
            if (inParent != null)
            {
                return inParent;
            }

            return other.GetComponentInChildren<ColorableObject>();
        }

        private void SpawnAura(Collider2D other)
        {
            if (auraPrefab == null)
            {
                return;
            }

            var hitPosition = other.ClosestPoint(transform.position);
            var aura = Instantiate(auraPrefab, hitPosition, Quaternion.identity);
            aura.Configure(auraScaleMultiplier);
        }
    }
}
