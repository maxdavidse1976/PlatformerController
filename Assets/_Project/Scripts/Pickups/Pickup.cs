using System.Collections;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] int _healSize;
        public PickupType pickupType;
        public enum PickupType
        {
            SmallAmbrosia,
            BigAmbrosia
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.CompareTag("Player");
            if (player)
            {
                PlayerHealth.Instance.HealPlayer(_healSize);
            }
        }

    }
}
