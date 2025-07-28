using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class DamageDealer : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private int baseDamage = 10;
        [SerializeField] private DamageType damageType = DamageType.Physical;
        [SerializeField] private LayerMask targetLayers = -1;
        [SerializeField] private bool dealDamageOnContact = true;
        [SerializeField] private float damageCooldown = 1f;
        
        [Header("Knockback")]
        [SerializeField] private bool applyKnockback = true;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockbackDuration = 0.3f;
        
        private float lastDamageTime;
        
        public System.Action<Health, int> OnDamageDealt;
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (dealDamageOnContact)
            {
                TryDealDamage(other.gameObject);
            }
        }
        
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (dealDamageOnContact && Time.time >= lastDamageTime + damageCooldown)
            {
                TryDealDamage(other.gameObject);
            }
        }
        
        public virtual bool TryDealDamage(GameObject target)
        {
            // Check if we can damage this target
            if (!CanDamageTarget(target))
                return false;
                
            // Check cooldown
            if (Time.time < lastDamageTime + damageCooldown)
                return false;
                
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth == null)
                return false;
                
            // Calculate final damage
            int finalDamage = CalculateDamage(targetHealth);
            
            // Deal damage
            targetHealth.TakeDamage(finalDamage);
            
            // Apply knockback if enabled
            if (applyKnockback)
            {
                ApplyKnockback(target);
            }
            
            // Update damage time
            lastDamageTime = Time.time;
            
            // Trigger events
            OnDamageDealt?.Invoke(targetHealth, finalDamage);
            
            // Log for debugging
            Debug.Log($"{gameObject.name} dealt {finalDamage} {damageType} damage to {target.name}");
            
            return true;
        }
        
        protected virtual bool CanDamageTarget(GameObject target)
        {
            // Check if target is on correct layer
            if ((targetLayers.value & (1 << target.layer)) == 0)
                return false;
                
            // Don't damage ourselves
            if (target == gameObject)
                return false;
                
            // Additional custom logic can be added here
            return true;
        }
        
        protected virtual int CalculateDamage(Health target)
        {
            // Base damage calculation - can be overridden for more complex systems
            int finalDamage = baseDamage;
            
            // Add damage type modifiers, resistances, etc. here in the future
            
            return finalDamage;
        }
        
        protected virtual void ApplyKnockback(GameObject target)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb == null) return;
            
            Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
            targetRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        
        public void SetDamage(int newDamage)
        {
            baseDamage = newDamage;
        }
        
        public void SetDamageType(DamageType newType)
        {
            damageType = newType;
        }
        
        public int GetDamage() => baseDamage;
        public DamageType GetDamageType() => damageType;
    }
}