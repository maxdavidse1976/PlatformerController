using UnityEngine;
using UnityEngine.EventSystems;

namespace DragonspiritGames.PlatformerController
{
    public class UIButton : MonoBehaviour, ISelectHandler
    {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _buttonDescriptionClip;
        
        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log($"Play button audio {_buttonDescriptionClip.name}");
            _audioSource.PlayOneShot(_buttonDescriptionClip);
        }
    }
}
