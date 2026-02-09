using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class RainAudioSystem : MonoBehaviour
    {
        // This can only exist once.
        public static RainAudioSystem Instance { get; private set; }

        [Header("Ambient Rain")]
        [SerializeField] AudioClip _ambientRainClip;
        [SerializeField] float _maxAmbientVolume = 0.4f;

        [Header("Impact audio clips")]
        [SerializeField] AudioClip[] _roofClips;
        [SerializeField] AudioClip[] _metalClips;
        [SerializeField] AudioClip[] _waterClips;

        [Header("Settings")]
        [SerializeField, Range(0f, 1f)] float _impactChance = 0.05f; // We don't want to constantly play audio whenever a drop hits something.

        [SerializeField] float _impactVolume = 0.3f;

        AudioSource _ambientSource;
        float _targetAmbientVolume;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            _ambientSource = gameObject.AddComponent<AudioSource>();
            _ambientSource.loop = true;
            _ambientSource.spatialBlend = 0f; // 2D sound
            _ambientSource.volume = 0f; // Initially silent

            // Only play the ambient rain if we have a clip assigned.
            if (_ambientRainClip != null)
            {
                _ambientSource.clip = _ambientRainClip;
                _ambientSource.Play();
            }
        }

        void Update()
        {
            _ambientSource.volume = Mathf.Lerp(_ambientSource.volume, _targetAmbientVolume, Time.deltaTime);
        }

        // Make sure we smoothly adjust the ambient volume.
        public void SetAmbientRainIntensity(float intensity)
        {
            _targetAmbientVolume = Mathf.Clamp01(intensity) * _maxAmbientVolume;
        }

        public void PlayImpact(RainSurfaceType surfaceType, Vector2 position)
        {
            if (Random.value > _impactChance) return;

            AudioClip clip = GetClip(surfaceType);
            if (clip == null) return; // No clip for this surface type.

            AudioSource.PlayClipAtPoint(clip, position, _impactVolume);
        }

        AudioClip GetClip(RainSurfaceType surfaceType)
        {
            return surfaceType switch
            {
                RainSurfaceType.Roof => _roofClips.Length > 0 ? _roofClips[Random.Range(0, _roofClips.Length)] : null,
                RainSurfaceType.Metal => _metalClips.Length > 0 ? _metalClips[Random.Range(0, _metalClips.Length)] : null,
                RainSurfaceType.Water => _waterClips.Length > 0 ? _waterClips[Random.Range(0, _waterClips.Length)] : null,
                _ => null,
            };
        }
    }
}
