using BalloonBloom.Coloring;
using BalloonBloom.Core;
using BalloonBloom.UI;
using UnityEngine;

namespace BalloonBloom.Player
{
    /// <summary>
    /// Emits visual pollen particles and spawns simple gameplay pollen droplets.
    /// </summary>
    public sealed class PollenEmitter : MonoBehaviour
    {
        [Header("Visual Particle Systems")]
        [SerializeField] private ParticleSystem[] visualPollenSystems;

        [Header("Emission")]
        [SerializeField] private PollenParticle pollenParticlePrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] [Min(0.1f)] private float particlesPerSecond = 16f;
        [SerializeField] [Min(0.01f)] private float pollenCostPerSecond = 14f;

        [Header("Spawn Spread")]
        [SerializeField] [Min(0f)] private float horizontalSpread = 0.25f;
        [SerializeField] [Tooltip("Degrees from scene +X axis. -90 looks straight down, -110 tips slightly backwards.")]
        [Range(-179f, 179f)] private float emissionAngleDegrees = -90f;
        [SerializeField] [Tooltip("Initial speed along emission direction (magnitude).")]
        [Min(0f)] private float initialDownwardSpeed = 1.5f;

        private PollenResource _resource;
        private float _spawnAccumulator;
        private bool _isEmitting;

        private void Start()
        {
            _resource = LevelManager.Instance != null ? LevelManager.Instance.PollenResource : null;
            SetVisualEmission(false);
        }

        private void Update()
        {
            _isEmitting = IsPressing();
            if (!_isEmitting || _resource == null)
            {
                SetVisualEmission(false);
                return;
            }

            var hasRemaining = _resource.Consume(pollenCostPerSecond * Time.deltaTime);
            if (!hasRemaining)
            {
                SetVisualEmission(false);
                return;
            }

            SetVisualEmission(true);
            SpawnGameplayParticles();
        }

        private void SpawnGameplayParticles()
        {
            if (pollenParticlePrefab == null || spawnPoint == null)
            {
                return;
            }

            _spawnAccumulator += particlesPerSecond * Time.deltaTime;
            while (_spawnAccumulator >= 1f)
            {
                _spawnAccumulator -= 1f;
                var offsetX = Random.Range(-horizontalSpread, horizontalSpread);
                var spawnPosition = spawnPoint.position + new Vector3(offsetX, 0f, 0f);
                var droplet = Instantiate(pollenParticlePrefab, spawnPosition, Quaternion.identity);
                var radians = emissionAngleDegrees * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
                droplet.Initialize(direction * initialDownwardSpeed);
            }
        }

        private bool IsPressing()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                return touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
            }

            return Input.GetMouseButton(0);
        }

        private void SetVisualEmission(bool enabled)
        {
            if (visualPollenSystems == null || visualPollenSystems.Length == 0)
            {
                return;
            }

            for (var i = 0; i < visualPollenSystems.Length; i++)
            {
                var ps = visualPollenSystems[i];
                if (ps == null)
                {
                    continue;
                }

                var emission = ps.emission;
                emission.enabled = enabled;

                if (enabled)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Play();
                    }
                }
            }
        }
    }
}
