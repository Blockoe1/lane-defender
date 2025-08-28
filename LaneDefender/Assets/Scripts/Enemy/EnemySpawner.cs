/*****************************************************************************
// File Name : EnemySpawner.cs
// Author : Brandon Koederitz
// Creation Date : August 26, 2025
// Last Modified : August 26, 2025
//
// Brief Description : Continually spawns enemies on the lanes throughout the game.
*****************************************************************************/
using System.Collections;
using UnityEngine;

namespace LaneDefender
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private float spawnDelay;

        private bool isSpawning;

        /// <summary>
        /// Start our spawning coroutine on awake.
        /// </summary>
        private void Awake()
        {
            StartCoroutine(SpawnRoutine(enemyPrefabs, spawnDelay));
        }

        /// <summary>
        /// Continually spawns a set array of enemy prefabs on the lanes.
        /// </summary>
        /// <param name="enemyPrefabs">The prefabs of the enemies to randomly spawn.</param>
        /// <param name="spawnDelay">The amount of time between each enemy being spawned.</param>
        /// <returns>Coroutine.</returns>
        private IEnumerator SpawnRoutine(GameObject[] enemyPrefabs, float spawnDelay)
        {
            static GameObject GetRandomPrefab(GameObject[] prefabs)
            {
                int randomIndex = Random.Range(0, prefabs.Length);
                return prefabs[randomIndex];
            }
            // Cache references.
            Vector3 spawnPos;
            int randomLaneIndex;

            // Wait a frame so that anything that runs in awake can set up.
            yield return null;

            isSpawning = true;
            while (isSpawning)
            {
                // Spawns the enemies at the spawner's X position
                spawnPos = transform.position;

                // Snaps the spawned enemy to one of the lanes.
                randomLaneIndex = Random.Range(0, LaneManager.Lanes.Length - 1);
                spawnPos.y = LaneManager.Lanes[randomLaneIndex];

                Instantiate(GetRandomPrefab(enemyPrefabs), spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        /// <summary>
        /// Stops the spawner from spawning more monsters.
        /// </summary>
        [ContextMenu("Stop Spawning")]
        public void StopSpawning()
        {
            isSpawning = false;
        }
    }
}
