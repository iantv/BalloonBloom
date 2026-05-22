using UnityEngine;

namespace BalloonBloom.World
{
    /// <summary>
    /// Loops horizontal tiles to create seamless moving backgrounds.
    /// </summary>
    public sealed class SeamlessScroller : MonoBehaviour
    {
        [SerializeField] [Min(0.1f)] private float tileWidth = 20f;
        [SerializeField] [Min(0f)] private float scrollSpeed = 2f;
        [SerializeField] private Transform[] tiles;

        private void Update()
        {
            if (tiles == null || tiles.Length == 0)
            {
                return;
            }

            var delta = Vector3.left * (scrollSpeed * Time.deltaTime);
            for (var i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != null)
                {
                    tiles[i].Translate(delta, Space.World);
                }
            }

            RecycleTiles();
        }

        private void RecycleTiles()
        {
            if (tiles.Length < 2)
            {
                return;
            }

            var leftMost = GetLeftMostTile();
            var rightMost = GetRightMostTile();
            if (leftMost == null || rightMost == null)
            {
                return;
            }

            // Recycle when the RIGHT EDGE of the leftmost tile exits the left side of the camera.
            var leftRightEdge = GetTileRightEdge(leftMost);
            if (leftRightEdge > GetCameraLeftEdge())
            {
                return;
            }

            // Place leftmost tile so its LEFT EDGE touches the RIGHT EDGE of rightmost tile.
            var rightRightEdge = GetTileRightEdge(rightMost);
            var leftHalfWidth  = GetTileHalfWidth(leftMost);

            leftMost.position = new Vector3(
                rightRightEdge + leftHalfWidth,
                leftMost.position.y,
                leftMost.position.z);
        }

        /// <summary>Right-most world X of a tile, using SpriteRenderer bounds when available.</summary>
        private float GetTileRightEdge(Transform tile)
        {
            var sr = tile.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                return sr.bounds.max.x;
            }

            return tile.position.x + tileWidth * 0.5f;
        }

        /// <summary>Half-width of a tile in world units, using SpriteRenderer bounds when available.</summary>
        private float GetTileHalfWidth(Transform tile)
        {
            var sr = tile.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                return sr.bounds.extents.x;
            }

            return tileWidth * 0.5f;
        }

        /// <summary>World X of the camera's left edge (falls back to anchor - tileWidth).</summary>
        private float GetCameraLeftEdge()
        {
            var cam = Camera.main;
            if (cam != null && cam.orthographic)
            {
                return cam.transform.position.x - cam.orthographicSize * cam.aspect;
            }

            return transform.position.x - tileWidth;
        }

        private Transform GetLeftMostTile()
        {
            Transform leftMost = null;
            for (var i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                if (tile == null)
                {
                    continue;
                }

                if (leftMost == null || tile.position.x < leftMost.position.x)
                {
                    leftMost = tile;
                }
            }

            return leftMost;
        }

        private Transform GetRightMostTile()
        {
            Transform rightMost = null;
            for (var i = 0; i < tiles.Length; i++)
            {
                var tile = tiles[i];
                if (tile == null)
                {
                    continue;
                }

                if (rightMost == null || tile.position.x > rightMost.position.x)
                {
                    rightMost = tile;
                }
            }

            return rightMost;
        }
    }
}
