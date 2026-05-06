using System.Collections.Generic;
using UnityEngine;

namespace BalloonBloom.World
{
    /// <summary>
    /// Endless house spawner with pooled cleanup by lifetime distance.
    /// </summary>
    public sealed class HouseSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] housePrefabs;
        [SerializeField] [Min(0.1f)] private float spawnInterval = 1.8f;
        [SerializeField] private Vector2 spawnYRange = new Vector2(-3.5f, -1.5f);
        [SerializeField] private float spawnX = 12f;
        [SerializeField] private float despawnX = -14f;
        [SerializeField] [Min(0f)] private float houseScrollSpeed = 2f;

        private readonly List<GameObject> _spawnedHouses = new List<GameObject>();
        private float _timer;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= spawnInterval)
            {
                _timer = 0f;
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

            var scroller = house.GetComponent<WorldScroller>();
            if (scroller == null)
            {
                scroller = house.AddComponent<WorldScroller>();
            }

            // Keep houses moving and cleaned up consistently.
            scroller.Configure(houseScrollSpeed, despawnX);
            _spawnedHouses.Add(house);
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
    }
}
