using System.Collections;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [RequireComponent(typeof(AudioSource))]
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

        public void PlayClip(AudioClip clip, float direction)
        {
            _audioSource.panStereo = direction;
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
            StartCoroutine(WaitForClipToFinish(clip, direction));
        }

        IEnumerator WaitForClipToFinish(AudioClip clip, float direction)
        {
            yield return new WaitForSeconds(clip.length);
        }

        public void StopClip(AudioClip clip)
        {
            _audioSource.Stop();
        }
    }
}
