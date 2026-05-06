using UnityEngine;

namespace BalloonBloom.World
{
    /// <summary>
    /// Moves world objects left to simulate forward balloon movement.
    /// </summary>
    public sealed class WorldScroller : MonoBehaviour
    {
        [SerializeField] [Min(0f)] private float scrollSpeed = 2f;
        [SerializeField] private float destroyX = -15f;
        [SerializeField] private bool autoDestroy = true;

        private void Update()
        {
            transform.Translate(Vector3.left * (scrollSpeed * Time.deltaTime), Space.World);

            if (autoDestroy && transform.position.x <= destroyX)
            {
                Destroy(gameObject);
            }
        }

        public void Configure(float speed, float newDestroyX)
        {
            scrollSpeed = Mathf.Max(0f, speed);
            destroyX = newDestroyX;
        }
    }
}
