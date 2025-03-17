using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioCue : MonoBehaviour
    {
        AudioSource _audioSource;
        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (AccessibilityManager.Instance.PlayAudioCues)
            {
                _audioSource.Play();
            }
        }
    }
}
