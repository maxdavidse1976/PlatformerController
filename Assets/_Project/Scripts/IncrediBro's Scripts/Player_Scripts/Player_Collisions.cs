using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Collisions : MonoBehaviour
    {
        Player_Stats player_stats;

        [SerializeField] private float raycastDistance;
        [SerializeField] private LayerMask groundCollisionLayers;

        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
        }

        private void Update()
        {
            player_stats.M_OnGround = IsGrounded();
        }

        bool IsGrounded()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, raycastDistance, groundCollisionLayers);

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
