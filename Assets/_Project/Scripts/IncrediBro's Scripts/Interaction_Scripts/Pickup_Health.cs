using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Pickup_Health : Interactable
    {
        [SerializeField] private int m_healAmount;

        public override void Interact()
        {
            Player_Stats.Instance.AddHealth(m_healAmount);
            Debug.Log("Added Health("+m_healAmount+") to " + Player_Stats.Instance);
        }
    }
}
