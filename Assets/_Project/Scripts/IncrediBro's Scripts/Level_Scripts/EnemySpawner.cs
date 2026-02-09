using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField] GameObject _enemyPrefab;
        [SerializeField] EnemyData[] _possibleEnemyTypes;
        [SerializeField] Transform[] _spawnPoints;
        [SerializeField] int _maxEnemies = 5;
        [SerializeField] float _spawnCooldown = 3f;
        [SerializeField] bool _spawnOnStart = true;
        [SerializeField] bool _respawnEnemies = true;
        
        [Header("Spawn Conditions")]
        [SerializeField] float _playerDistanceThreshold = 10f;
        [SerializeField] bool _requirePlayerInRange = true;
        
        List<Enemy> _spawnedEnemies = new List<Enemy>();
        float _lastSpawnTime;
        Transform _player;
        
        void Start()
        {
            // Find player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                _player = playerObj.transform;
            
            // Set default spawn points if none assigned
            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                _spawnPoints = new Transform[] { transform };
            }
            
            if (_spawnOnStart)
            {
                SpawnInitialEnemies();
            }
        }
        
        void Update()
        {
            // Clean up destroyed enemies
            _spawnedEnemies.RemoveAll(enemy => enemy == null);
            
            // Check if we should spawn more enemies
            if (_respawnEnemies && CanSpawn())
            {
                SpawnEnemy();
            }
        }
        
        void SpawnInitialEnemies()
        {
            int enemiesToSpawn = Mathf.Min(_maxEnemies, _spawnPoints.Length);
            
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemyAtPoint(i % _spawnPoints.Length);
            }
        }
        
        bool CanSpawn()
        {
            // Check if we're under the limit
            if (_spawnedEnemies.Count >= _maxEnemies)
                return false;
                
            // Check cooldown
            if (Time.time < _lastSpawnTime + _spawnCooldown)
                return false;
                
            // Check if player is in range (if required)
            if (_requirePlayerInRange && _player != null)
            {
                bool playerInRange = false;
                foreach (var spawnPoint in _spawnPoints)
                {
                    if (Vector2.Distance(_player.position, spawnPoint.position) <= _playerDistanceThreshold)
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
        
        void SpawnEnemy()
        {
            // Find a suitable spawn point
            int spawnIndex = Random.Range(0, _spawnPoints.Length);
            
            // Make sure player isn't too close to spawn point
            if (_player != null && Vector2.Distance(_player.position, _spawnPoints[spawnIndex].position) < 2f)
            {
                // Try to find a different spawn point
                for (int i = 0; i < _spawnPoints.Length; i++)
                {
                    if (Vector2.Distance(_player.position, _spawnPoints[i].position) >= 2f)
                    {
                        spawnIndex = i;
                        break;
                    }
                }
            }
            
            SpawnEnemyAtPoint(spawnIndex);
        }
        
        void SpawnEnemyAtPoint(int spawnIndex)
        {
            if (spawnIndex < 0 || spawnIndex >= _spawnPoints.Length)
                return;
                
            if (_enemyPrefab == null)
                return;
                
            // Spawn the enemy
            GameObject spawnedObj = Instantiate(_enemyPrefab, _spawnPoints[spawnIndex].position, _spawnPoints[spawnIndex].rotation);
            Enemy enemy = spawnedObj.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                // Assign random enemy data if multiple types available
                if (_possibleEnemyTypes != null && _possibleEnemyTypes.Length > 0)
                {
                    EnemyData randomEnemyData = _possibleEnemyTypes[Random.Range(0, _possibleEnemyTypes.Length)];
                    enemy.SetEnemyData(randomEnemyData);
                }
                
                // Subscribe to death event for cleanup
                enemy.OnEnemyDeath += OnEnemyDied;
                
                _spawnedEnemies.Add(enemy);
                _lastSpawnTime = Time.time;
                
                Debug.Log($"Spawned enemy at {_spawnPoints[spawnIndex].position}");
            }
            else
            {
                Debug.LogWarning("Enemy prefab doesn't have Enemy component!");
            }
        }
        
        void OnEnemyDied(Enemy deadEnemy)
        {
            _spawnedEnemies.Remove(deadEnemy);
            Debug.Log($"Enemy died: {deadEnemy.name}");
        }
        
        public void SpawnEnemyManually()
        {
            if (_spawnedEnemies.Count < _maxEnemies)
            {
                SpawnEnemy();
            }
        }
        
        public void SpawnSpecificEnemy(EnemyData enemyData)
        {
            if (_spawnedEnemies.Count >= _maxEnemies || _enemyPrefab == null)
                return;
                
            int spawnIndex = Random.Range(0, _spawnPoints.Length);
            GameObject spawnedObj = Instantiate(_enemyPrefab, _spawnPoints[spawnIndex].position, _spawnPoints[spawnIndex].rotation);
            Enemy enemy = spawnedObj.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                enemy.SetEnemyData(enemyData);
                enemy.OnEnemyDeath += OnEnemyDied;
                _spawnedEnemies.Add(enemy);
                _lastSpawnTime = Time.time;
            }
        }
        
        public void ClearAllEnemies()
        {
            foreach (var enemy in _spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            _spawnedEnemies.Clear();
        }
        
        public int GetSpawnedEnemyCount() => _spawnedEnemies.Count;
        public int GetMaxEnemies() => _maxEnemies;
        
        private void OnDrawGizmosSelected()
        {
            if (_spawnPoints != null)
            {
                Gizmos.color = Color.blue;
                foreach (var point in _spawnPoints)
                {
                    if (point != null)
                    {
                        Gizmos.DrawWireCube(point.position, Vector3.one);
                        
                        // Draw player distance threshold
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(point.position, _playerDistanceThreshold);
                        Gizmos.color = Color.blue;
                    }
                }
            }
        }
    }
}