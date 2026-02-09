using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Pickup_Health : Interactable
    {
        [SerializeField] int _healAmount;

        public override void Interact()
        {
            Player_Stats.Instance.AddHealth(_healAmount);
        }
    }
}
