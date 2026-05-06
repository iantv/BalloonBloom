using System.Collections.Generic;
using UnityEngine;

namespace BalloonBloom.World
{
    /// <summary>
    /// Endless house spawner with pooled cleanup by lifetime distance.
    /// </summary>
    public sealed class HouseSpawner : MonoBehaviour
    {
        [Header("Main Houses")]
        [SerializeField] private GameObject[] housePrefabs;
        [SerializeField] private Vector2 spawnYRange = new Vector2(-3.5f, -1.5f);
        [SerializeField] private float spawnX = 12f;
        [SerializeField] private float despawnX = -14f;
        [SerializeField] [Min(0f)] private float houseScrollSpeed = 2f;
        [SerializeField] private Vector2 scaleRange = new Vector2(0.85f, 1.25f);

        [Header("Spacing")]
        [SerializeField] private Vector2 spacingRange = new Vector2(2f, 4.5f);

        [Header("Optional Decorations")]
        [SerializeField] private GameObject[] decorationPrefabs;
        [SerializeField] [Range(0f, 1f)] private float decorationSpawnChance = 0.4f;
        [SerializeField] private Vector2 decorationYOffsetRange = new Vector2(0.1f, 0.8f);
        [SerializeField] private Vector2 decorationScaleRange = new Vector2(0.8f, 1.4f);

        private readonly List<GameObject> _spawnedHouses = new List<GameObject>();
        private float _spawnTimer;
        private float _nextSpawnDelay;

        private void Start()
        {
            _nextSpawnDelay = SampleSpawnDelay();
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _nextSpawnDelay)
            {
                _spawnTimer = 0f;
                _nextSpawnDelay = SampleSpawnDelay();
                SpawnHouse();
            }

            CleanupDestroyed();
        }

        private void SpawnHouse()
        {
            if (housePrefabs == null || housePrefabs.Length == 0)
            {
                return;
            }

            var prefab = housePrefabs[Random.Range(0, housePrefabs.Length)];
            if (prefab == null)
            {
                return;
            }

            var spawnY = Random.Range(spawnYRange.x, spawnYRange.y);
            var spawnPosition = new Vector3(spawnX, spawnY, 0f);
            var house = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            var scale = Random.Range(scaleRange.x, scaleRange.y);
            house.transform.localScale = Vector3.one * scale;

            var scroller = house.GetComponent<WorldScroller>();
            if (scroller == null)
            {
                scroller = house.AddComponent<WorldScroller>();
            }

            // Keep houses moving and cleaned up consistently.
            scroller.Configure(houseScrollSpeed, despawnX);
            _spawnedHouses.Add(house);
            TrySpawnDecorationNear(house.transform.position, scale);
        }

        private void CleanupDestroyed()
        {
            for (var i = _spawnedHouses.Count - 1; i >= 0; i--)
            {
                if (_spawnedHouses[i] == null)
                {
                    _spawnedHouses.RemoveAt(i);
                }
            }
        }

        private float SampleSpawnDelay()
        {
            var spacing = Random.Range(spacingRange.x, spacingRange.y);
            var speed = Mathf.Max(0.1f, houseScrollSpeed);
            return spacing / speed;
        }

        private void TrySpawnDecorationNear(Vector3 anchorPosition, float houseScale)
        {
            if (decorationPrefabs == null || decorationPrefabs.Length == 0)
            {
                return;
            }

            if (Random.value > decorationSpawnChance)
            {
                return;
            }

            var prefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Length)];
            if (prefab == null)
            {
                return;
            }

            var yOffset = Random.Range(decorationYOffsetRange.x, decorationYOffsetRange.y);
            var decorationPosition = new Vector3(anchorPosition.x, anchorPosition.y + yOffset, anchorPosition.z);
            var decoration = Instantiate(prefab, decorationPosition, Quaternion.identity, transform);

            var decorationScale = Random.Range(decorationScaleRange.x, decorationScaleRange.y) * houseScale;
            decoration.transform.localScale = Vector3.one * decorationScale;

            var scroller = decoration.GetComponent<WorldScroller>();
            if (scroller == null)
            {
                scroller = decoration.AddComponent<WorldScroller>();
            }

            scroller.Configure(houseScrollSpeed, despawnX);
            _spawnedHouses.Add(decoration);
        }
    }
}
