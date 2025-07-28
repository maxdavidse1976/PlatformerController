using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability Data")]
    public class AbilityData : ScriptableObject
    {
        [Header("Basic Info")]
        public string abilityName;
        [TextArea(2, 4)]
        public string description;
        public Sprite abilityIcon;
        
        [Header("Damage & Effects")]
        public int baseDamage = 10;
        public DamageType damageType = DamageType.Physical;
        public float range = 2f;
        public float cooldown = 1f;
        public float castTime = 0.2f;
        
        [Header("Area of Effect")]
        public bool isAreaOfEffect = false;
        public float aoeRadius = 1.5f;
        public LayerMask targetLayers = -1;
        
        [Header("Projectile (if applicable)")]
        public bool isProjectile = false;
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;
        public float projectileLifetime = 3f;
        
        [Header("Visual & Audio")]
        public GameObject castEffect;
        public GameObject impactEffect;
        public AudioClip castSound;
        public AudioClip impactSound;
        
        [Header("Status Effects")]
        public StatusEffect[] statusEffects;
    }

    [System.Serializable]
    public class StatusEffect
    {
        public StatusEffectType effectType;
        public float duration;
        public int effectValue; // damage per tick, slow amount, etc.
        public float tickRate = 1f; // for damage over time effects
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Lightning,
        Poison,
        Healing // negative damage = healing
    }

    public enum StatusEffectType
    {
        None,
        Poison,
        Burn,
        Freeze,
        Slow,
        Stun,
        Heal
    }
}