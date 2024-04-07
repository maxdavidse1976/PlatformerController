using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Interactions : MonoBehaviour
    {
        private Player_Stats player_stats;
        private Interactable m_currentInteractable;

        private void Awake()
        {
            player_stats = GetComponent<Player_Stats>();
        }

        private void Update()
        {
            if(m_currentInteractable && m_currentInteractable.transform.parent == null && player_stats.M_Input.InteractEndedThisFrame())
            {
                m_currentInteractable.Interact();
            }

            if (player_stats.M_HoldThrowTransform.childCount > 0 && player_stats.M_Input.InteractEndedThisFrame())
            {
                player_stats.M_HoldThrowTransform.GetComponentInChildren<Throwable>().Throw();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.TryGetComponent<Interactable>(out m_currentInteractable);

            m_currentInteractable?.EnableInteractGFX();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (m_currentInteractable?.GetInteractableType() != InteractableType.Health)
            {
                m_currentInteractable?.DisableInteractGFX();
            }
            m_currentInteractable = null;
        }
    }
}
