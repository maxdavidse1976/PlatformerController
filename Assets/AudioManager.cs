using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        public static AudioManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayClip(AudioClip clip)
        {
            _audioSource.clip = clip;
            if (!_audioSource.isPlaying)
                _audioSource.Play();
            
        }

        public void StopClip(AudioClip clip)
        {
            _audioSource.Stop();
        }
    }
}
