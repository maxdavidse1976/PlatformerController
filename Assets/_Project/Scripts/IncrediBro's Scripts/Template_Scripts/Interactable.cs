using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public enum InteractableType
    {
        Health,
        Holdable,
        Portal
    }

    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] protected GameObject m_interactGFX;
        [SerializeField] protected InteractableType m_interactableType;

        public InteractableType GetInteractableType() { return m_interactableType; }

        public abstract void Interact();

        public virtual void EnableInteractGFX()
        {
            m_interactGFX?.SetActive(true);

            if(m_interactableType == InteractableType.Health)
            {
                Interact();
                m_interactGFX.transform.parent = null;
                Destroy(gameObject);
            }
            else if (m_interactableType == InteractableType.Portal)
            {
                Interact();
            }
        }

        public virtual void DisableInteractGFX()
        {
            m_interactGFX?.SetActive(false);
        }
    }
}
