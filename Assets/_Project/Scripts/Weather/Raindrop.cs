using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DragonspiritGames.PlatformerController
{
    public class Raindrop : MonoBehaviour
    {
        Vector2 _baseVelocity;
        Vector2 _externalVelocity;

        [Header("Interaction")]
        [SerializeField] LayerMask _bounceLayer; // Which objects should cause a bounce
        [SerializeField] LayerMask _waterLayer; // Used to define water surfaces
        [SerializeField] float _bounceMultiplier = -.3f; // How much velocity is retained on bounce
        [SerializeField] float _splashDuration = .2f;

        bool _isActive = true;

        public void Initialize(Vector2 startPosition, float fallSpeed)
        {
            transform.position = startPosition;
            _baseVelocity = Vector2.down * fallSpeed;
            _externalVelocity = Vector2.zero;
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void SetExternalVelocity(Vector2 externalVelocity)
        {
            _externalVelocity = externalVelocity;
        }
        private void Update()
        {
            if (!_isActive) return;

            Vector2 finalVelocity = _baseVelocity + _externalVelocity;

            // Predict next position
            Vector2 nextPosition = (Vector2)transform.position + finalVelocity * Time.deltaTime;

            // Make sure we raycast down to see if we hit anything
            RaycastHit2D hit = Physics2D.Linecast(transform.position, nextPosition, _bounceLayer | _waterLayer);

            if (hit.collider != null)
            {
                Debug.Log("We are colliding");
                // Determine what type of surface we are hitting
                var surface = hit.collider.GetComponent<RainSurface>();

                Debug.Log($"Colliding with surface: {surface}");
                // If we have a surface and an audio system, play impact sound
                if (surface != null && RainAudioSystem.Instance != null)
                {
                    RainAudioSystem.Instance.PlayImpact(surface.SurfaceType, hit.point);
                }

                if (((1 << hit.collider.gameObject.layer) & _waterLayer) != 0)
                {
                    // Hit water surface - create splash and deactivate
                    StartCoroutine(SplashAndDeactivate(hit.point));
                    return;
                }

                // Hit ground or other object - handle bounce
                HandleGroundImpact(hit.point);
            }
            transform.position += (Vector3)finalVelocity * Time.deltaTime;
        }

        void HandleGroundImpact(Vector2 hitPoint)
        {
            // Make most of the raindrops dissapear on impact, with only a few bouncing
            if (Random.value < 0.7f)
            {
                Deactivate();
                return;
            }
            // Small sideways movement, no more than that.
            Vector2 scatter = new Vector2(
                Random.Range(-0.1f, 0.1f),
                Random.Range(0.02f, 0.08f) // Should be better, because of a very small upward bounce value
            );

            _baseVelocity = scatter;
            _externalVelocity = Vector2.zero;

            Invoke(nameof(Deactivate), 0.05f);
        }

        IEnumerator SplashAndDeactivate(Vector2 position)
        {
            transform.position = position;
            // Initiate a splash effect here, which is something we will need to create later
            // Still need to decide whether that should be a sprite or a particle effect.
            yield return new WaitForSeconds(_splashDuration);
            Invoke(nameof(Deactivate), _splashDuration);
        }

        void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}