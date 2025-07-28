using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class StatusEffectController : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = true;
        
        private List<ActiveStatusEffect> activeEffects = new List<ActiveStatusEffect>();
        private Health healthComponent;
        
        public System.Action<StatusEffect> OnStatusEffectApplied;
        public System.Action<StatusEffect> OnStatusEffectRemoved;
        
        private void Awake()
        {
            healthComponent = GetComponent<Health>();
        }
        
        public void ApplyStatusEffect(StatusEffect effect)
        {
            if (effect.effectType == StatusEffectType.None)
                return;
                
            // Check if we already have this effect type
            ActiveStatusEffect existingEffect = activeEffects.Find(e => e.effect.effectType == effect.effectType);
            
            if (existingEffect != null)
            {
                // Refresh the duration
                existingEffect.remainingDuration = effect.duration;
                if (showDebugInfo)
                    Debug.Log($"Refreshed {effect.effectType} on {gameObject.name}");
            }
            else
            {
                // Add new effect
                ActiveStatusEffect newEffect = new ActiveStatusEffect
                {
                    effect = effect,
                    remainingDuration = effect.duration,
                    nextTickTime = Time.time + effect.tickRate
                };
                
                activeEffects.Add(newEffect);
                StartCoroutine(ProcessStatusEffect(newEffect));
                
                OnStatusEffectApplied?.Invoke(effect);
                
                if (showDebugInfo)
                    Debug.Log($"Applied {effect.effectType} to {gameObject.name} for {effect.duration} seconds");
            }
        }
        
        private IEnumerator ProcessStatusEffect(ActiveStatusEffect activeEffect)
        {
            while (activeEffect.remainingDuration > 0)
            {
                yield return null;
                
                // Update remaining duration
                activeEffect.remainingDuration -= Time.deltaTime;
                
                // Process tick-based effects
                if (Time.time >= activeEffect.nextTickTime)
                {
                    ProcessEffectTick(activeEffect);
                    activeEffect.nextTickTime = Time.time + activeEffect.effect.tickRate;
                }
            }
            
            // Remove effect when duration expires
            RemoveStatusEffect(activeEffect);
        }
        
        private void ProcessEffectTick(ActiveStatusEffect activeEffect)
        {
            switch (activeEffect.effect.effectType)
            {
                case StatusEffectType.Poison:
                case StatusEffectType.Burn:
                    if (healthComponent != null)
                    {
                        healthComponent.TakeDamage(activeEffect.effect.effectValue);
                        if (showDebugInfo)
                            Debug.Log($"{gameObject.name} took {activeEffect.effect.effectValue} {activeEffect.effect.effectType} damage");
                    }
                    break;
                    
                case StatusEffectType.Heal:
                    if (healthComponent != null)
                    {
                        healthComponent.AddHealth(activeEffect.effect.effectValue);
                        if (showDebugInfo)
                            Debug.Log($"{gameObject.name} healed {activeEffect.effect.effectValue} health");
                    }
                    break;
                    
                case StatusEffectType.Slow:
                    // This would need to be handled by movement scripts
                    break;
                    
                case StatusEffectType.Freeze:
                case StatusEffectType.Stun:
                    // These would need to be handled by movement/input scripts
                    break;
            }
        }
        
        private void RemoveStatusEffect(ActiveStatusEffect activeEffect)
        {
            activeEffects.Remove(activeEffect);
            OnStatusEffectRemoved?.Invoke(activeEffect.effect);
            
            if (showDebugInfo)
                Debug.Log($"Removed {activeEffect.effect.effectType} from {gameObject.name}");
        }
        
        public void RemoveStatusEffectType(StatusEffectType effectType)
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                if (activeEffects[i].effect.effectType == effectType)
                {
                    RemoveStatusEffect(activeEffects[i]);
                }
            }
        }
        
        public void ClearAllStatusEffects()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                RemoveStatusEffect(activeEffects[i]);
            }
        }
        
        public bool HasStatusEffect(StatusEffectType effectType)
        {
            return activeEffects.Exists(e => e.effect.effectType == effectType);
        }
        
        public float GetStatusEffectRemainingDuration(StatusEffectType effectType)
        {
            ActiveStatusEffect effect = activeEffects.Find(e => e.effect.effectType == effectType);
            return effect != null ? effect.remainingDuration : 0f;
        }
        
        public StatusEffect[] GetActiveStatusEffects()
        {
            StatusEffect[] effects = new StatusEffect[activeEffects.Count];
            for (int i = 0; i < activeEffects.Count; i++)
            {
                effects[i] = activeEffects[i].effect;
            }
            return effects;
        }
        
        // For movement scripts to check if movement should be affected
        public bool IsMovementImpaired()
        {
            return HasStatusEffect(StatusEffectType.Freeze) || 
                   HasStatusEffect(StatusEffectType.Stun) || 
                   HasStatusEffect(StatusEffectType.Slow);
        }
        
        public float GetMovementSpeedMultiplier()
        {
            if (HasStatusEffect(StatusEffectType.Freeze) || HasStatusEffect(StatusEffectType.Stun))
                return 0f;
                
            if (HasStatusEffect(StatusEffectType.Slow))
            {
                ActiveStatusEffect slowEffect = activeEffects.Find(e => e.effect.effectType == StatusEffectType.Slow);
                return 1f - (slowEffect.effect.effectValue / 100f); // Assuming effectValue is percentage reduction
            }
            
            return 1f;
        }
    }
    
    [System.Serializable]
    public class ActiveStatusEffect
    {
        public StatusEffect effect;
        public float remainingDuration;
        public float nextTickTime;
    }
}