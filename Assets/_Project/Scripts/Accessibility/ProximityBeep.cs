using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class ProximityBeep : MonoBehaviour
    {
        [Header("References")]
        public Transform player;       // Assign your player here
        public Transform target;       // Assign your target here
        public AudioSource audioSource; // Assign the AudioSource with your beep clip

        [Header("Settings")]
        public float maxDistance = 5f;    // Farthest distance for beep to start
        public float minInterval = 0.1f;   // Fastest beep interval (close to target)
        public float maxInterval = 1.5f;   // Slowest beep interval (far from target)

        private float nextBeepTime = 0f;

        void Update()
        {
            float distance = Vector3.Distance(player.position, target.position);

            if (distance > maxDistance)
                return;

            // Map distance to beep interval
            float t = Mathf.InverseLerp(maxDistance, 0f, distance);
            float interval = Mathf.Lerp(minInterval, maxInterval, 1f - t);

            if (Time.time >= nextBeepTime)
            {
                audioSource.PlayOneShot(audioSource.clip);
                nextBeepTime = Time.time + interval;
            }
        }
    }
}
