using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class SmoothScreenShake : MonoBehaviour
    {
        Vector3 _originalPosition;
        float _timeRemaining;
        float _magnitude;

        void Start()
        {
            _originalPosition = transform.localPosition;
        }

        void Update()
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                float shakeAmount = _magnitude * (_timeRemaining / Mathf.Max(_timeRemaining, 0.001f));
                transform.localPosition = _originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            }
            else
            {
                transform.localPosition = _originalPosition;
            }
        }

        public void Shake(float intensity, float duration)
        {
            _timeRemaining = duration;
            _magnitude = intensity;
        }
    }
}
