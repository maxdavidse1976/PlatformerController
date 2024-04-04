using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Climbing : MonoBehaviour
    {
        private Player_Stats player_stats;
        private ClimbableObject m_currentClimbableObject;
        
        [SerializeField] private float m_climbSpeed = 4f;
        
        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
        }

        private void Update()
        {
            if (m_currentClimbableObject)
            {
                float verticalInput = player_stats.M_Input.GetVerticalAxis();
                Rigidbody2D rb = player_stats.M_Rigidbody;

                // Check if the player is pressing up or down to climb
                if (verticalInput != 0f)
                {
                    player_stats.M_OnLadder = true;
                    transform.position = new Vector3(m_currentClimbableObject.transform.position.x, transform.position.y, transform.position.z);
                    rb.velocity = new Vector2(0f, verticalInput * m_climbSpeed);
                    rb.gravityScale = 0f;  // Disable gravity while climbing
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                }

                if (m_currentClimbableObject.allowSideWays)
                {
                    float horizontalInput = player_stats.M_Input.GetHorizontalAxis();
                    rb.velocity = new Vector2(horizontalInput * m_climbSpeed, rb.velocity.x);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Climbable")
            {
                m_currentClimbableObject = other.GetComponent<ClimbableObject>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Climbable")
            {
                m_currentClimbableObject = null;
                player_stats.M_Rigidbody.gravityScale = 1f;
                player_stats.M_OnLadder = false;
            }
        }
    }
}
