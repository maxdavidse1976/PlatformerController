using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Movement : MonoBehaviour
    {
        Player_Stats _player_stats;
        [SerializeField, Range(0f, 100f)] float _maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] float _maxAirAcceleration = 20f;

        Vector2 _inputDirection;
        Vector2 _desiredVelocity;

        float _acceleration;
        float _maxSpeedChange;

        bool _onGround;

        void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
        }

        void Update()
        {
            if (_player_stats.OnLadder && !_player_stats.OnGround) return;

            _inputDirection.x = _player_stats.Input.GetHorizontalAxis();
            _desiredVelocity = new Vector2(_inputDirection.x, 0f) * Mathf.Max(_maxSpeed - _player_stats.Friction, 0f);
        }

        void FixedUpdate()
        {
            if (_player_stats.OnLadder && !_player_stats.OnGround) return;

            Move();
        }

        void Move()
        {
            _onGround = _player_stats.OnGround;
            Vector2 _velocity = _player_stats.Rigidbody.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _player_stats.Rigidbody.velocity = _velocity;
        }
    }
}
