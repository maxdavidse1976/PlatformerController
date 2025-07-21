using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_MovingPlatformSupport : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.CompareTag("Moving Platform"))
            {
                transform.SetParent(collision.transform);
                transform.localScale = Vector3.one; 
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Moving Platform"))
            {
                transform.SetParent(null);
            }
        }
    }
}
