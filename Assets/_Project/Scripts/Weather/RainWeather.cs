using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace DragonspiritGames.PlatformerController
{
    public class RainWeather : MonoBehaviour, IWeather
    {
        [Header("Rain Settings")]
        [SerializeField] int _maxRaindrops = 200;
        [SerializeField] float _fallSpeed = 6f;
        [SerializeField] float _spawnRate = 30f;
        [SerializeField] float _rainIntensity = 1f;

        [Header("References")]
        [SerializeField] WindSystem _windSystem;
        [SerializeField] Raindrop _raindropPrefab;

        [Header("Spawn Area")]
        [SerializeField] float _spawnWidth = 20f;
        [SerializeField] float _spawnHeight = 10f;
        [SerializeField] float _despawnY = -10f;

        readonly List<Raindrop> _raindropPool = new();

        float _spawnTimer;
        float _intensity = 1f;
        bool _isActive;

        void Awake()
        {
            for (int i = 0; i < _maxRaindrops; i++)
            {
                Raindrop raindrop = Instantiate(_raindropPrefab);
                raindrop.gameObject.SetActive(false);
                _raindropPool.Add(raindrop);
            }
        }

        void Update()
        {
            if (!_isActive || _intensity <= 0f) return;

            if (RainAudioSystem.Instance != null)
            {
                RainAudioSystem.Instance.SetAmbientRainIntensity(_rainIntensity);
            }

            HandleSpawning();
            ApplyWindToActiveDrops();
            DespawnDrops();
        }

        public void StartWeather()
        {
            _isActive = true;
        }

        public void StopWeather()
        {
            _isActive = false;
            foreach (var raindrop in _raindropPool)
            {
                raindrop.gameObject.SetActive(false);
            }
        }

        public void SetIntensity(float intensity)
        {
            _intensity = Mathf.Clamp01(intensity);
        }

        void HandleSpawning()
        {
            _spawnTimer += Time.deltaTime * _intensity;

            float actualSpawnRate = 1f / _spawnRate;

            while (_spawnTimer >= actualSpawnRate)
            {
                _spawnTimer -= actualSpawnRate;
                SpawnRaindrop();
            }
        }

        void SpawnRaindrop()
        {
            Raindrop drop = FindInactiveDrop();
            if (drop == null) return;

            Vector2 spawnPosition = new(
                Random.Range(-_spawnWidth * .5f,
                _spawnWidth * .5f),
                _spawnHeight
            );

            drop.Initialize((Vector2)transform.position + spawnPosition, _fallSpeed);
        }

        Raindrop FindInactiveDrop()
        {
            return _raindropPool.Find(r => !r.gameObject.activeSelf);
        }

        void ApplyWindToActiveDrops()
        {
            // Don't apply wind if there's no wind system attached
            if (_windSystem == null) return;

            // Raindrops are less affected by wind than other objects
            Vector2 windVelocity = _windSystem.WindVelocity * .5f;

            foreach (var drop in _raindropPool)
            {
                if (!drop.gameObject.activeSelf) continue;

                drop.SetExternalVelocity(windVelocity);
            }
        }

        void DespawnDrops()
        {
            foreach (var drop in _raindropPool)
            {
                if (!drop.gameObject.activeSelf) continue;
                
                if (drop.transform.position.y < _despawnY)
                {
                    drop.gameObject.SetActive(false);
                }
            }
        }
    }
}
