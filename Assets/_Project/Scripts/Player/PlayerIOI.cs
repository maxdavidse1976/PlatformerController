using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    public class PlayerIOI : MonoBehaviour
    {
        [SerializeField] float _rumbleDistance = 5f;
        [SerializeField] float _maxAudioDistance = 1;
        float _rumbleStrength = 0f;
        float _timer = 0f;

        void Update()
        {
            Transform ioi = FindIOI();
            float distance = ioi.position.x - transform.position.x;
            
            IsIOIWithinAudioRange(ioi, distance);

            if (ioi.position.x - transform.position.x <= _rumbleDistance)
            {
                _rumbleStrength = 1 / Mathf.Abs(distance);
                if (distance < 0)
                {
                    Gamepad.current?.SetMotorSpeeds(_rumbleStrength, 0);
                }
                else
                {
                    Gamepad.current?.SetMotorSpeeds(0, _rumbleStrength);
                }
            }
        }

        void IsIOIWithinAudioRange(Transform ioi, float distance)
        {
            Transform audioObject = ioi.Find("Audio");
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            Debug.Log($"Current distance: {distance}");
            if (Mathf.Abs(distance) > _maxAudioDistance)
            {
                audioSource.enabled = false;
            }
            else
            {
                audioSource.enabled = true;
            }
        }

        Transform FindIOI ()
        {
            GameObject ioiTransform = GameObject.FindWithTag("IOI");
            return ioiTransform.transform;
        }
    }
}
