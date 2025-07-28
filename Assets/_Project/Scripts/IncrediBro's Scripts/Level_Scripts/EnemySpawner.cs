using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private EnemyData[] possibleEnemyTypes;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private int maxEnemies = 5;
        [SerializeField] private float spawnCooldown = 3f;
        [SerializeField] private bool spawnOnStart = true;
        [SerializeField] private bool respawnEnemies = true;
        
        [Header("Spawn Conditions")]
        [SerializeField] private float playerDistanceThreshold = 10f;
        [SerializeField] private bool requirePlayerInRange = true;
        
        private List<Enemy> spawnedEnemies = new List<Enemy>();
        private float lastSpawnTime;
        private Transform player;
        
        private void Start()
        {
            // Find player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            
            // Set default spawn points if none assigned
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                spawnPoints = new Transform[] { transform };
            }
            
            if (spawnOnStart)
            {
                SpawnInitialEnemies();
            }
        }
        
        private void Update()
        {
            // Clean up destroyed enemies
            spawnedEnemies.RemoveAll(enemy => enemy == null);
            
            // Check if we should spawn more enemies
            if (respawnEnemies && CanSpawn())
            {
                SpawnEnemy();
            }
        }
        
        private void SpawnInitialEnemies()
        {
            int enemiesToSpawn = Mathf.Min(maxEnemies, spawnPoints.Length);
            
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemyAtPoint(i % spawnPoints.Length);
            }
        }
        
        private bool CanSpawn()
        {
            // Check if we're under the limit
            if (spawnedEnemies.Count >= maxEnemies)
                return false;
                
            // Check cooldown
            if (Time.time < lastSpawnTime + spawnCooldown)
                return false;
                
            // Check if player is in range (if required)
            if (requirePlayerInRange && player != null)
            {
                bool playerInRange = false;
                foreach (var spawnPoint in spawnPoints)
                {
                    if (Vector2.Distance(player.position, spawnPoint.position) <= playerDistanceThreshold)
                    {
                        playerInRange = true;
                        break;
                    }
                }
                
                if (!playerInRange)
                    return false;
            }
            
            return true;
        }
        
        private void SpawnEnemy()
        {
            // Find a suitable spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            
            // Make sure player isn't too close to spawn point
            if (player != null && Vector2.Distance(player.position, spawnPoints[spawnIndex].position) < 2f)
            {
                // Try to find a different spawn point
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (Vector2.Distance(player.position, spawnPoints[i].position) >= 2f)
                    {
                        spawnIndex = i;
                        break;
                    }
                }
            }
            
            SpawnEnemyAtPoint(spawnIndex);
        }
        
        private void SpawnEnemyAtPoint(int spawnIndex)
        {
            if (spawnIndex < 0 || spawnIndex >= spawnPoints.Length)
                return;
                
            if (enemyPrefab == null)
                return;
                
            // Spawn the enemy
            GameObject spawnedObj = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            Enemy enemy = spawnedObj.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                // Assign random enemy data if multiple types available
                if (possibleEnemyTypes != null && possibleEnemyTypes.Length > 0)
                {
                    EnemyData randomEnemyData = possibleEnemyTypes[Random.Range(0, possibleEnemyTypes.Length)];
                    enemy.SetEnemyData(randomEnemyData);
                }
                
                // Subscribe to death event for cleanup
                enemy.OnEnemyDeath += OnEnemyDied;
                
                spawnedEnemies.Add(enemy);
                lastSpawnTime = Time.time;
                
                Debug.Log($"Spawned enemy at {spawnPoints[spawnIndex].position}");
            }
            else
            {
                Debug.LogWarning("Enemy prefab doesn't have Enemy component!");
            }
        }
        
        private void OnEnemyDied(Enemy deadEnemy)
        {
            spawnedEnemies.Remove(deadEnemy);
            Debug.Log($"Enemy died: {deadEnemy.name}");
        }
        
        public void SpawnEnemyManually()
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
        
        public void SpawnSpecificEnemy(EnemyData enemyData)
        {
            if (spawnedEnemies.Count >= maxEnemies || enemyPrefab == null)
                return;
                
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnedObj = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            Enemy enemy = spawnedObj.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                enemy.SetEnemyData(enemyData);
                enemy.OnEnemyDeath += OnEnemyDied;
                spawnedEnemies.Add(enemy);
                lastSpawnTime = Time.time;
            }
        }
        
        public void ClearAllEnemies()
        {
            foreach (var enemy in spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            spawnedEnemies.Clear();
        }
        
        public int GetSpawnedEnemyCount() => spawnedEnemies.Count;
        public int GetMaxEnemies() => maxEnemies;
        
        private void OnDrawGizmosSelected()
        {
            if (spawnPoints != null)
            {
                Gizmos.color = Color.blue;
                foreach (var point in spawnPoints)
                {
                    if (point != null)
                    {
                        Gizmos.DrawWireCube(point.position, Vector3.one);
                        
                        // Draw player distance threshold
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(point.position, playerDistanceThreshold);
                        Gizmos.color = Color.blue;
                    }
                }
            }
        }
    }
}