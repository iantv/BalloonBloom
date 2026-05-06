using UnityEngine;

namespace BalloonBloom.World
{
    /// <summary>
    /// Moves a world layer left using its own parallax multiplier.
    /// </summary>
    public sealed class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float baseSpeed = 2f;
        [SerializeField] [Min(0f)] private float speedMultiplier = 1f;

        private void Update()
        {
            var speed = baseSpeed * speedMultiplier;
            transform.Translate(Vector3.left * (speed * Time.deltaTime), Space.World);
        }

        public void Configure(float newBaseSpeed, float newMultiplier)
        {
            baseSpeed = Mathf.Max(0f, newBaseSpeed);
            speedMultiplier = Mathf.Max(0f, newMultiplier);
        }
    }
}
