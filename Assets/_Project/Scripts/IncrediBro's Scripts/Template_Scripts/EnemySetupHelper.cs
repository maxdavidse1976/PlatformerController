using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class EnemySetupHelper : MonoBehaviour
    {
        [Header("Quick Enemy Setup")]
        [SerializeField] private string enemyName = "Basic Enemy";
        [SerializeField] private int health = 50;
        [SerializeField] private int contactDamage = 10;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private EnemyBehaviorType behaviorType = EnemyBehaviorType.Aggressive;
        
        [Space(10)]
        [Header("Create Sample Scriptable Objects")]
        [SerializeField] private bool createSampleAbilities = false;
        [SerializeField] private bool createSampleEnemyData = false;
        
        [ContextMenu("Setup Basic Enemy")]
        public void SetupBasicEnemy()
        {
            // Add required components if they don't exist
            if (GetComponent<Enemy>() == null)
                gameObject.AddComponent<Enemy>();
                
            if (GetComponent<AbilityController>() == null)
                gameObject.AddComponent<AbilityController>();
                
            if (GetComponent<StatusEffectController>() == null)
                gameObject.AddComponent<StatusEffectController>();
                
            if (GetComponent<DamageDealer>() == null)
            {
                DamageDealer damageDealer = gameObject.AddComponent<DamageDealer>();
                damageDealer.SetDamage(contactDamage);
            }
            
            // Setup collider for damage dealing
            if (GetComponent<Collider2D>() == null)
            {
                BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }
            
            // Setup rigidbody if not present
            if (GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
                rb.freezeRotation = true;
            }
            
            Debug.Log($"Basic enemy setup complete for {gameObject.name}");
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Create Sample Ability Data")]
        public void CreateSampleAbilityData()
        {
            // Create basic melee attack
            AbilityData meleeAttack = ScriptableObject.CreateInstance<AbilityData>();
            meleeAttack.abilityName = "Basic Melee Attack";
            meleeAttack.description = "A simple melee attack that deals physical damage.";
            meleeAttack.baseDamage = 15;
            meleeAttack.damageType = DamageType.Physical;
            meleeAttack.range = 1.5f;
            meleeAttack.cooldown = 1f;
            meleeAttack.castTime = 0.2f;
            meleeAttack.targetLayers = 1 << 6; // Player layer
            
            string path = "Assets/_Project/Scriptable Objects/BasicMeleeAttack.asset";
            UnityEditor.AssetDatabase.CreateAsset(meleeAttack, path);
            
            // Create fire projectile
            AbilityData fireProjectile = ScriptableObject.CreateInstance<AbilityData>();
            fireProjectile.abilityName = "Fire Projectile";
            fireProjectile.description = "Shoots a fireball that deals fire damage.";
            fireProjectile.baseDamage = 20;
            fireProjectile.damageType = DamageType.Fire;
            fireProjectile.range = 8f;
            fireProjectile.cooldown = 2f;
            fireProjectile.castTime = 0.5f;
            fireProjectile.isProjectile = true;
            fireProjectile.projectileSpeed = 8f;
            fireProjectile.projectileLifetime = 3f;
            fireProjectile.targetLayers = 1 << 6; // Player layer
            
            // Add burn status effect
            fireProjectile.statusEffects = new StatusEffect[]
            {
                new StatusEffect
                {
                    effectType = StatusEffectType.Burn,
                    duration = 3f,
                    effectValue = 5,
                    tickRate = 1f
                }
            };
            
            string firePath = "Assets/_Project/Scriptable Objects/FireProjectile.asset";
            UnityEditor.AssetDatabase.CreateAsset(fireProjectile, firePath);
            
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            
            Debug.Log("Sample ability data created!");
        }
        
        [ContextMenu("Create Sample Enemy Data")]
        public void CreateSampleEnemyData()
        {
            // Create basic enemy data
            EnemyData basicEnemy = ScriptableObject.CreateInstance<EnemyData>();
            basicEnemy.enemyName = enemyName;
            basicEnemy.description = "A basic enemy that attacks the player on sight.";
            basicEnemy.maxHealth = health;
            basicEnemy.contactDamage = contactDamage;
            basicEnemy.moveSpeed = moveSpeed;
            basicEnemy.detectionRange = 5f;
            basicEnemy.attackRange = 2f;
            basicEnemy.attackCooldown = 1.5f;
            basicEnemy.behaviorType = behaviorType;
            basicEnemy.chaseSpeed = moveSpeed + 1f;
            basicEnemy.wanderRadius = 3f;
            basicEnemy.wanderTimer = 2f;
            
            string path = $"Assets/_Project/Scriptable Objects/{enemyName.Replace(" ", "")}Data.asset";
            UnityEditor.AssetDatabase.CreateAsset(basicEnemy, path);
            
            // Create flying enemy data
            EnemyData flyingEnemy = ScriptableObject.CreateInstance<EnemyData>();
            flyingEnemy.enemyName = "Flying Scout";
            flyingEnemy.description = "A fast flying enemy that harasses the player.";
            flyingEnemy.maxHealth = 30;
            flyingEnemy.contactDamage = 8;
            flyingEnemy.moveSpeed = 4f;
            flyingEnemy.detectionRange = 7f;
            flyingEnemy.attackRange = 3f;
            flyingEnemy.attackCooldown = 1f;
            flyingEnemy.behaviorType = EnemyBehaviorType.Aggressive;
            flyingEnemy.chaseSpeed = 6f;
            flyingEnemy.canFly = true;
            flyingEnemy.wanderRadius = 5f;
            flyingEnemy.wanderTimer = 1.5f;
            
            string flyingPath = "Assets/_Project/Scriptable Objects/FlyingScoutData.asset";
            UnityEditor.AssetDatabase.CreateAsset(flyingEnemy, flyingPath);
            
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            
            Debug.Log("Sample enemy data created!");
        }
        #endif
    }
}