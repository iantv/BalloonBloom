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
            Debug.Log($"Hit: {other.name}");

            var colorable = other.GetComponentInParent<ColorableObject>();
            if (colorable != null)
            {
                colorable.ApplyPollenHit(hitIntensity);
            }

            Destroy(gameObject);
        }
    }
}
