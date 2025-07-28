using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] protected AbilityData[] abilities;
        [SerializeField] protected Transform castPoint;
        
        protected Dictionary<AbilityData, float> abilityCooldowns = new Dictionary<AbilityData, float>();
        protected bool isCasting = false;
        
        public System.Action<AbilityData> OnAbilityCast;
        public System.Action<AbilityData, GameObject> OnAbilityHit;
        
        protected virtual void Start()
        {
            // Initialize cooldowns
            foreach (var ability in abilities)
            {
                abilityCooldowns[ability] = 0f;
            }
        }
        
        protected virtual void Update()
        {
            // Update cooldowns
            var keys = new List<AbilityData>(abilityCooldowns.Keys);
            foreach (var ability in keys)
            {
                if (abilityCooldowns[ability] > 0)
                {
                    abilityCooldowns[ability] -= Time.deltaTime;
                }
            }
        }
        
        public virtual bool TryUseAbility(int abilityIndex, Vector2? targetPosition = null)
        {
            if (abilityIndex < 0 || abilityIndex >= abilities.Length)
                return false;
                
            return TryUseAbility(abilities[abilityIndex], targetPosition);
        }
        
        public virtual bool TryUseAbility(AbilityData ability, Vector2? targetPosition = null)
        {
            // Check if ability exists
            if (ability == null)
                return false;
                
            // Check if we're already casting
            if (isCasting)
                return false;
                
            // Check cooldown
            if (abilityCooldowns.ContainsKey(ability) && abilityCooldowns[ability] > 0)
                return false;
                
            // Start casting
            StartCoroutine(CastAbility(ability, targetPosition));
            return true;
        }
        
        protected virtual IEnumerator CastAbility(AbilityData ability, Vector2? targetPosition = null)
        {
            isCasting = true;
            
            // Play cast effect and sound
            if (ability.castEffect != null)
            {
                Instantiate(ability.castEffect, castPoint.position, castPoint.rotation);
            }
            
            if (ability.castSound != null)
            {
                AudioSource.PlayClipAtPoint(ability.castSound, transform.position);
            }
            
            // Wait for cast time
            yield return new WaitForSeconds(ability.castTime);
            
            // Execute the ability
            ExecuteAbility(ability, targetPosition);
            
            // Set cooldown
            abilityCooldowns[ability] = ability.cooldown;
            
            // Trigger event
            OnAbilityCast?.Invoke(ability);
            
            isCasting = false;
        }
        
        protected virtual void ExecuteAbility(AbilityData ability, Vector2? targetPosition = null)
        {
            if (ability.isProjectile)
            {
                FireProjectile(ability, targetPosition);
            }
            else if (ability.isAreaOfEffect)
            {
                AreaOfEffectAbility(ability, targetPosition);
            }
            else
            {
                DirectAbility(ability, targetPosition);
            }
        }
        
        protected virtual void FireProjectile(AbilityData ability, Vector2? targetPosition = null)
        {
            if (ability.projectilePrefab == null) return;
            
            Vector2 direction;
            if (targetPosition.HasValue)
            {
                direction = (targetPosition.Value - (Vector2)castPoint.position).normalized;
            }
            else
            {
                direction = transform.right; // Default to facing direction
            }
            
            GameObject projectile = Instantiate(ability.projectilePrefab, castPoint.position, Quaternion.LookRotation(Vector3.forward, direction));
            
            // Setup projectile damage
            DamageDealer projectileDamage = projectile.GetComponent<DamageDealer>();
            if (projectileDamage == null)
            {
                projectileDamage = projectile.AddComponent<DamageDealer>();
            }
            projectileDamage.SetDamage(ability.baseDamage);
            projectileDamage.SetDamageType(ability.damageType);
            
            // Setup projectile movement
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = direction * ability.projectileSpeed;
            }
            
            // Destroy projectile after lifetime
            Destroy(projectile, ability.projectileLifetime);
        }
        
        protected virtual void AreaOfEffectAbility(AbilityData ability, Vector2? targetPosition = null)
        {
            Vector2 center = targetPosition ?? (Vector2)castPoint.position;
            
            // Find all targets in AOE range
            Collider2D[] targets = Physics2D.OverlapCircleAll(center, ability.aoeRadius, ability.targetLayers);
            
            foreach (var target in targets)
            {
                DealAbilityDamage(ability, target.gameObject);
            }
            
            // Play impact effect
            if (ability.impactEffect != null)
            {
                Instantiate(ability.impactEffect, center, Quaternion.identity);
            }
        }
        
        protected virtual void DirectAbility(AbilityData ability, Vector2? targetPosition = null)
        {
            // For direct abilities, find the closest target in range
            Vector2 center = castPoint.position;
            Collider2D[] targets = Physics2D.OverlapCircleAll(center, ability.range, ability.targetLayers);
            
            float closestDistance = float.MaxValue;
            Collider2D closestTarget = null;
            
            foreach (var target in targets)
            {
                float distance = Vector2.Distance(center, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
            
            if (closestTarget != null)
            {
                DealAbilityDamage(ability, closestTarget.gameObject);
                
                // Play impact effect at target
                if (ability.impactEffect != null)
                {
                    Instantiate(ability.impactEffect, closestTarget.transform.position, Quaternion.identity);
                }
            }
        }
        
        protected virtual void DealAbilityDamage(AbilityData ability, GameObject target)
        {
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth == null) return;
            
            // Deal damage (or healing if negative)
            if (ability.damageType == DamageType.Healing)
            {
                targetHealth.AddHealth(ability.baseDamage);
            }
            else
            {
                targetHealth.TakeDamage(ability.baseDamage);
            }
            
            // Apply status effects
            StatusEffectController statusController = target.GetComponent<StatusEffectController>();
            if (statusController != null && ability.statusEffects != null)
            {
                foreach (var statusEffect in ability.statusEffects)
                {
                    statusController.ApplyStatusEffect(statusEffect);
                }
            }
            
            // Play impact sound
            if (ability.impactSound != null)
            {
                AudioSource.PlayClipAtPoint(ability.impactSound, target.transform.position);
            }
            
            // Trigger event
            OnAbilityHit?.Invoke(ability, target);
        }
        
        public bool IsAbilityOnCooldown(int abilityIndex)
        {
            if (abilityIndex < 0 || abilityIndex >= abilities.Length)
                return true;
                
            return abilityCooldowns[abilities[abilityIndex]] > 0;
        }
        
        public float GetAbilityCooldownRemaining(int abilityIndex)
        {
            if (abilityIndex < 0 || abilityIndex >= abilities.Length)
                return 0f;
                
            return Mathf.Max(0f, abilityCooldowns[abilities[abilityIndex]]);
        }
        
        public void AddAbility(AbilityData newAbility)
        {
            var abilityList = new List<AbilityData>(abilities);
            abilityList.Add(newAbility);
            abilities = abilityList.ToArray();
            abilityCooldowns[newAbility] = 0f;
        }
        
        public AbilityData[] GetAbilities() => abilities;
    }
}