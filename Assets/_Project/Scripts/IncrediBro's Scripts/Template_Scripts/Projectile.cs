using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private bool destroyOnImpact = true;
        [SerializeField] private LayerMask impactLayers = -1;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject impactEffect;
        [SerializeField] private TrailRenderer trail;
        
        private Rigidbody2D projectileRigidbody;
        private DamageDealer damageDealer;
        private bool hasImpacted = false;
        
        private void Awake()
        {
            projectileRigidbody = GetComponent<Rigidbody2D>();
            damageDealer = GetComponent<DamageDealer>();
            
            // Auto-destroy after lifetime
            Destroy(gameObject, lifetime);
        }
        
        private void Start()
        {
            // Set initial velocity
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = transform.right * speed;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if we should impact with this object
            if ((impactLayers.value & (1 << other.gameObject.layer)) == 0)
                return;
                
            if (hasImpacted && destroyOnImpact)
                return;
                
            OnImpact(other.gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if we should impact with this object
            if ((impactLayers.value & (1 << collision.gameObject.layer)) == 0)
                return;
                
            if (hasImpacted && destroyOnImpact)
                return;
                
            OnImpact(collision.gameObject);
        }
        
        private void OnImpact(GameObject impactTarget)
        {
            hasImpacted = true;
            
            // Try to deal damage
            if (damageDealer != null)
            {
                damageDealer.TryDealDamage(impactTarget);
            }
            
            // Spawn impact effect
            if (impactEffect != null)
            {
                GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(effect, 2f); // Clean up effect after 2 seconds
            }
            
            // Stop movement
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = Vector2.zero;
            }
            
            // Disable trail if it exists
            if (trail != null)
            {
                trail.enabled = false;
            }
            
            // Destroy projectile if set to do so
            if (destroyOnImpact)
            {
                // Delay destruction slightly to allow effects to play
                Destroy(gameObject, 0.1f);
            }
        }
        
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = transform.right * speed;
            }
        }
        
        public void SetDirection(Vector2 direction)
        {
            transform.right = direction;
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = direction * speed;
            }
        }
        
        public void SetLifetime(float newLifetime)
        {
            lifetime = newLifetime;
        }
    }
}