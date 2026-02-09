using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class WindSystem : MonoBehaviour
    {
        [SerializeField] Vector2 _windDirection = Vector2.right;
        [SerializeField] float _windStrength = 0f;
        [SerializeField] float _gustStrength = 2f;
        [SerializeField] float _gustDuration = 0.5f;
        [SerializeField] float _minGustInterval = 3f;
        [SerializeField] float _maxGustInterval = 7f;

        float _gustTimer;
        float _nextGustTime;
        Vector2 _gustVelocity;

        void Start()
        {
            ScheduleNextGust();
        }

        void ScheduleNextGust()
        {
            _nextGustTime = Random.Range(_minGustInterval, _maxGustInterval);
        }

        void Update()
        {
            // Countdown to next gust
            _nextGustTime -= Time.deltaTime;

            if (_nextGustTime <= 0f)
            {
                TriggerRandomGust();
                ScheduleNextGust();
            }

            // Handle active gust
            if (_gustTimer > 0f)
            {
                _gustTimer -= Time.deltaTime;
                if (_gustTimer <= 0f)
                {
                    _gustVelocity = Vector2.zero;
                }
            }
        }

        void TriggerRandomGust()
        {
            Vector2 gustDirection = _windDirection;

            // Small random variation so gusts aren't idenitical
            gustDirection += new Vector2(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.1f, 0.1f)
            );

            _gustVelocity = gustDirection.normalized * _gustStrength;
            _gustTimer = _gustDuration;
        }

        public Vector2 WindVelocity => _windDirection.normalized * _windStrength + _gustVelocity;

        public void SetWind(Vector2 newDirection, float newStrength)
        {
            _windDirection = newDirection.normalized;
            _windStrength = Mathf.Max(0f, newStrength);
        }

        public void ClearWind()
        {
            _windStrength = 0f;
        }
    }
}
