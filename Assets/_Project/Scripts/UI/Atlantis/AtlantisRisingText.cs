using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class AtlantisRisingText : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float _startDepth = -100f;          // How far below the world does this effect start
        [SerializeField] float _duration = 2.5f;            // Total rising time
        [SerializeField] AnimationCurve _riseCurve;         // Controls speed feeling
        
        [Header("Shake (Camera) Trigger")]
        [SerializeField] float _shakeTriggerPercent = 0.8f; // When should the shake start to happen (80% through rising)
        [SerializeField] float _shakeIntensity = 0.3f;      // How strong should the shake feel
        [SerializeField] float _shakeDuration = 0.6f;       // How long should the shake last

        [Header("Wobble/Weight Effect")] 
        [SerializeField] float _wobbleAmount = 1.5f;          // Tilt of the text like it is a heavy/object rocking
        [SerializeField] float _wobbleSpeed = 4f;
        
        [Header("Audio settings")]
        [SerializeField] AudioSource _audioSource;
        [SerializeField] float _audioStartPercentage = 0f;
        bool _audioStarted = false;
        
        Transform _camera;
        Vector3 _startPosition;
        Vector3 _targetPosition;
        float _timer;
        bool _shakeTriggered;
        
        void Start()
        {
            // Cache the camera for shaking
            _camera = Camera.main.transform;
            
            // Save the final intended position
            _targetPosition = transform.position;
            
            // Start underwater (below)
            _startPosition = new Vector3(_targetPosition.x, _targetPosition.y + _startDepth, _targetPosition.z);
            transform.position = _startPosition;
            
            // If somehow we forgot a curve, make a default "weighty" one
            if (_riseCurve == null || _riseCurve.keys.Length == 0)
            {
                _riseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            }
        }

        void Update()
        {
            if (_timer < _duration)
            {
                _timer += Time.deltaTime;
                float t = Mathf.Clamp01(_timer / _duration);
                float curved = _riseCurve.Evaluate(t);
                
                // Smooth heavy rise
                transform.position = Vector3.Lerp(_startPosition, _targetPosition, curved);
                
                // Subtle heavy "rocking"
                float wobble = Mathf.Sin(Time.time * _wobbleSpeed) * _wobbleAmount * (1f - t);
                transform.rotation = Quaternion.Euler(0, 0, wobble);
                
                // Fire shake near breakthrough moment
                if (!_shakeTriggered && t >= _shakeTriggerPercent)
                {
                    _shakeTriggered = true;
                    
                    // Trigger camera shake if Shake component can be found
                    var shaker = _camera.GetComponent<SmoothScreenShake>();
                    if (shaker != null)
                    {
                        shaker.Shake(_shakeIntensity, _shakeDuration);
                    }
                }
                
                // Audio trigger
                if (!_audioStarted && t >= _audioStartPercentage)
                {
                    _audioStarted = true;
                    if (_audioSource != null)
                    {
                        _audioSource.Play();
                        _audioSource.volume = Mathf.Lerp(0f, 1f, t);
                    }
                }
            }
            else
            {
                // Settle to final rotation
                transform.rotation = Quaternion.identity;
                _audioSource.Stop();
                _audioStarted = false;
            }
        }
    }
}
