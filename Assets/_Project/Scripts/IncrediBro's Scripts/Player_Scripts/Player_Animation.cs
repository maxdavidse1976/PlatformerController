using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Animation : MonoBehaviour
    {
        Player_Stats player_stats;

        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
        }

        private void Update()
        {
            AnimateMovements();
            AnimateJumps();

            if (player_stats.M_Rigidbody.velocity.x < -0.2f)
            {
                //player_stats.M_SpriteRenderer.flipX = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (player_stats.M_Rigidbody.velocity.x > 0.2f)
            {
                //player_stats.M_SpriteRenderer.flipX = false;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        void AnimateMovements()
        {
            if (Mathf.Abs(player_stats.M_Input.GetHorizontalAxis()) > Mathf.Epsilon && player_stats.M_OnGround)
            {
                player_stats.M_Animator.SetBool("isRunning", true);
                //AudioManager.Instance.PlayClip(_stepsClips[7]);
            }
            else
            {
                player_stats.M_Animator.SetBool("isRunning", false);
                //AudioManager.Instance.StopClip(_stepsClips[7]);
            }
        }

        void AnimateJumps()
        {
            player_stats.M_Animator.SetFloat("yVelocity", player_stats.M_Rigidbody.velocity.y);
            player_stats.M_Animator.SetBool("isGrounded", player_stats.M_OnGround);
        }
    }
}
