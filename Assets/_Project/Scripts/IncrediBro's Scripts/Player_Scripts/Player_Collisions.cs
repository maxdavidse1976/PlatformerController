using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Collisions : MonoBehaviour
    {
        Player_Stats _player_stats;

        [SerializeField] float _raycastDistance;
        [SerializeField] LayerMask _groundCollisionLayers;

        void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
        }

        void Update()
        {
            _player_stats.OnGround = IsGrounded();
        }

        bool IsGrounded()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, _raycastDistance, _groundCollisionLayers);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
