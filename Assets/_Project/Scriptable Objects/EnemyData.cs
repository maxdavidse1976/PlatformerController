using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game Data/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Basic Info")]
        public string enemyName;
        [TextArea(3, 5)]
        public string description;
        public Sprite enemySprite;

        [Header("Health & Combat")]
        public int maxHealth = 100;
        public int contactDamage = 10;
        public float attackRange = 2f;
        public float attackCooldown = 1.5f;
        
        [Header("Movement")]
        public float moveSpeed = 3f;
        public float detectionRange = 5f;
        public float jumpForce = 8f;
        public bool canFly = false;
        
        [Header("AI Behavior")]
        public EnemyBehaviorType behaviorType = EnemyBehaviorType.Aggressive;
        public float wanderRadius = 3f;
        public float wanderTimer = 2f;
        public float chaseSpeed = 5f;
        public float retreatDistance = 1f;
        
        [Header("Abilities")]
        public AbilityData[] abilities;
        
        [Header("Drops")]
        public DropData[] possibleDrops;
    }

    [System.Serializable]
    public class DropData
    {
        public GameObject dropPrefab;
        [Range(0f, 1f)]
        public float dropChance = 0.3f;
        public int minAmount = 1;
        public int maxAmount = 1;
    }

    public enum EnemyBehaviorType
    {
        Passive,     // Won't attack unless attacked first
        Aggressive,  // Will chase and attack player on sight
        Territorial, // Attacks when player enters their area
        Coward,      // Runs away from player
        Guardian     // Stands ground and attacks when in range
    }
}