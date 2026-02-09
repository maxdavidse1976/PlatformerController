using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Interactions : MonoBehaviour
    {
        Player_Stats _player_stats;
        Interactable _currentInteractable;

        void Awake()
        {
            _player_stats = GetComponent<Player_Stats>();
        }

        void Update()
        {
            if(_currentInteractable && _currentInteractable.transform.parent == null && _player_stats.Input.InteractEndedThisFrame())
            {
                _currentInteractable.Interact();
            }

            if (_player_stats.HoldThrowTransform.childCount > 0 && _player_stats.Input.InteractEndedThisFrame())
            {
                _player_stats.HoldThrowTransform.GetComponentInChildren<Throwable>().Throw();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            other.TryGetComponent<Interactable>(out _currentInteractable);

            _currentInteractable?.EnableInteractGFX();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (_currentInteractable?.GetInteractableType() != InteractableType.Health)
            {
                _currentInteractable?.DisableInteractGFX();
            }
            _currentInteractable = null;
        }
    }
}
