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

            if (leftMost.position.x <= transform.position.x - tileWidth)
            {
                leftMost.position = new Vector3(
                    rightMost.position.x + tileWidth,
                    leftMost.position.y,
                    leftMost.position.z);
            }
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
