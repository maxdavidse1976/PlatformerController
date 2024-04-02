using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float jumpHeight = 3f;
        [SerializeField, Range(0f, 5f)] private int maxAirJumps = 1;
        [SerializeField, Range(0f, 5f)] private float downwardGravity = 3f;
        [SerializeField, Range(0f, 5f)] private float upwardGravity = 1.7f;
        [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.2f;
        [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.2f;

        [SerializeField] private float maxJumpTime = 0.5f; // Maximum time the jump button can be held down
        [SerializeField] private float variableJumpHeightMultiplier = 0.5f; // Multiplier for variable jump height
        [SerializeField] private float jumpForce = 10f;
        private Player_Stats player_stats;

        private bool isJumping;
        private bool canJump;
        private float jumpTime;
        private float jumpBufferTimer;
        private float coyoteTimer;
        private int remainingJumps;

        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
            remainingJumps = maxAirJumps;
        }

        private void Update()
        {
            if (player_stats.M_Input.JumpPressedThisFrame())
            {
                Jump();
            }

            // Handle variable jump height
            if (isJumping && player_stats.M_Input.JumpPressed() && jumpTime < maxJumpTime)
            {
                player_stats.M_Rigidbody.velocity = new Vector2(player_stats.M_Rigidbody.velocity.x, jumpForce * (1 + variableJumpHeightMultiplier * (1 - jumpTime / maxJumpTime)));
                jumpTime += Time.deltaTime;
            }
            else if (isJumping && player_stats.M_Input.JumpEndedThisFrame())
            {
                isJumping = false;
            }

            if (jumpTime >= maxJumpTime)
            {
                isJumping = false;
            }

            if (player_stats.M_Grounded)
            {
                remainingJumps = maxAirJumps;
                jumpTime = 0;
            }
        }
        private void FixedUpdate()
        {
            if (isJumping && player_stats.M_Input.JumpPressed() && player_stats.M_Rigidbody.velocity.y > 0)
            {
                player_stats.M_Rigidbody.velocity = new Vector2(player_stats.M_Rigidbody.velocity.x, jumpForce * (1 + variableJumpHeightMultiplier * (1 - jumpTime / maxJumpTime)));
            }
        }
        void Jump()
        {
            if (player_stats.M_Grounded || remainingJumps > 0)
            {
                player_stats.M_Rigidbody.velocity = new Vector2(player_stats.M_Rigidbody.velocity.x, jumpForce);
                isJumping = true;
                jumpTime = 0f;

                if (!player_stats.M_Grounded)
                {
                    remainingJumps--;
                }
            }
        }

        private void HandleJump()
        {
            // Handle variable jump height
            if (isJumping && player_stats.M_Rigidbody.velocity.y > 0)
            {
                player_stats.M_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * upwardGravity * (variableJumpHeightMultiplier - 1) * Time.deltaTime;
            }
            else if (player_stats.M_Rigidbody.velocity.y < 0)
            {
                player_stats.M_Rigidbody.velocity += Vector2.down * Physics2D.gravity.y * downwardGravity * Time.deltaTime;
            }

            // Perform jump if jump is allowed
            if (canJump && remainingJumps > 0)
            {
                Jump();
            }
        }
    }
}
