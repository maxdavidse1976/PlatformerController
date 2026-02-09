using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Animation : MonoBehaviour
    {
        Player_Stats _player_stats;

        private void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
        }

        private void Update()
        {
            AnimateMovements();
            AnimateJumps();

            if (_player_stats.Rigidbody.velocity.x < -0.2f)
            {
                _player_stats.SpriteRenderer.flipX = true;
            }
            else if (_player_stats.Rigidbody.velocity.x > 0.2f)
            {
                _player_stats.SpriteRenderer.flipX = false;
            }
        }

        void AnimateMovements()
        {
            if (Mathf.Abs(_player_stats.Input.GetHorizontalAxis()) > Mathf.Epsilon && _player_stats.OnGround)
            {
                _player_stats.Animator.SetBool("isRunning", true);
                //AudioManager.Instance.PlayClip(_stepsClips[7]);
            }
            else
            {
                _player_stats.Animator.SetBool("isRunning", false);
                //AudioManager.Instance.StopClip(_stepsClips[7]);
            }
        }

        void AnimateJumps()
        {
            _player_stats.Animator.SetFloat("yVelocity", _player_stats.Rigidbody.velocity.y);
            _player_stats.Animator.SetBool("isGrounded", _player_stats.OnGround);
        }
    }
}
