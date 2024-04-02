using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Movement : MonoBehaviour
    {
        Player_Stats player_stats;
        [SerializeField, Range(0f, 100f)] private float m_maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float m_maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float m_maxAirAcceleration = 20f;

        Vector2 _inputDirection;
        Vector2 _desiredVelocity;

        float _acceleration;
        float _maxSpeedChange;

        bool _onGround;

        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
        }

        void Update()
        {
            _inputDirection.x = player_stats.M_Input.RetrieveMoveInput();
            _desiredVelocity = new Vector2(_inputDirection.x, 0f) * Mathf.Max(m_maxSpeed - player_stats.M_Friction, 0f);
        }

        private void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            _onGround = player_stats.M_Grounded;
            Vector2 _velocity = player_stats.M_Rigidbody.velocity;

            _acceleration = _onGround ? m_maxAcceleration : m_maxAirAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            player_stats.M_Rigidbody.velocity = _velocity;
        }
    }
}
