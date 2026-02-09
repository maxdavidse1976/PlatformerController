using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Climbing : MonoBehaviour
    {
        Player_Stats _player_stats;
        ClimbableObject _currentClimbableObject;
        
        [SerializeField] float _climbSpeed = 4f;
        
        void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
        }

        void Update()
        {
            if (_currentClimbableObject)
            {
                float verticalInput = _player_stats.Input.GetVerticalAxis();
                Rigidbody2D rb = _player_stats.Rigidbody;

                // Check if the player is pressing up or down to climb
                if (verticalInput != 0f)
                {
                    _player_stats.OnLadder = true;
                    transform.position = new Vector3(_currentClimbableObject.transform.position.x, transform.position.y, transform.position.z);
                    rb.velocity = new Vector2(0f, verticalInput * _climbSpeed);
                    rb.gravityScale = 0f;  // Disable gravity while climbing
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                }

                if (_currentClimbableObject.allowSideWays)
                {
                    float horizontalInput = _player_stats.Input.GetHorizontalAxis();
                    rb.velocity = new Vector2(horizontalInput * _climbSpeed, rb.velocity.x);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Climbable")
            {
                _currentClimbableObject = other.GetComponent<ClimbableObject>();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Climbable")
            {
                _currentClimbableObject = null;
                _player_stats.Rigidbody.gravityScale = 1f;
                _player_stats.OnLadder = false;
            }
        }
    }
}
