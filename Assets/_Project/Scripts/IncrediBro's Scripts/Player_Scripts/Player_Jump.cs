using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 5f)] private int maxAirJumps = 1;

        [SerializeField] float _maxJumpTime = 0.5f; // Maximum time the jump button can be held down
        [SerializeField] float _variableJumpHeightMultiplier = 0.5f; // Multiplier for variable jump height
        [SerializeField] float _jumpForce = 10f;
        Player_Stats _player_stats;

        bool _isJumping;
        float _jumpTime;
        int _remainingJumps;

        void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
            _remainingJumps = maxAirJumps;
        }

        void Update()
        {
            if (_player_stats.Input.JumpPressedThisFrame())
            {
                Jump();
            }

            // Handle variable jump height
            if (_isJumping && _jumpTime < _maxJumpTime)
            {
                _jumpTime += Time.deltaTime;
            }

            if (_isJumping && _player_stats.Input.JumpEndedThisFrame())
            {
                _isJumping = false;
            }

            if (_jumpTime >= _maxJumpTime)
            {
                _isJumping = false;
            }

            if (_player_stats.OnGround)
            {
                _remainingJumps = maxAirJumps;
                _jumpTime = 0;
            }
        }
        void FixedUpdate()
        {
            if (_isJumping && _player_stats.Input.JumpPressedThisFrame() && _player_stats.Rigidbody.velocity.y > 0)
            {
                _player_stats.Rigidbody.velocity = new Vector2(_player_stats.Rigidbody.velocity.x, _jumpForce * (1 + _variableJumpHeightMultiplier * (1 - _jumpTime / _maxJumpTime)));
            }
        }
        void Jump()
        {
            if (_player_stats.OnGround || _remainingJumps > 0)
            {
                _player_stats.Rigidbody.velocity = new Vector2(_player_stats.Rigidbody.velocity.x, _jumpForce);
                _isJumping = true;
                _jumpTime = 0f;

                if (!_player_stats.OnGround)
                {
                    _remainingJumps--;
                }
            }
        }
    }
}
