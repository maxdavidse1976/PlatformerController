using System.Collections;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Enemy : Health
    {
        [Header("Enemy Configuration")]
        [SerializeField] protected EnemyData enemyData;
        
        [Header("Components")]
        [SerializeField] protected Rigidbody2D enemyRigidbody;
        [SerializeField] protected Animator enemyAnimator;
        [SerializeField] protected SpriteRenderer enemySpriteRenderer;
        [SerializeField] protected Collider2D enemyCollider;
        
        [Header("Detection")]
        [SerializeField] protected LayerMask playerLayer = 1 << 6; // Default to Player layer
        [SerializeField] protected Transform detectionPoint;
        
        protected AbilityController abilityController;
        protected StatusEffectController statusEffectController;
        protected DamageDealer contactDamage;
        
        // AI State
        protected EnemyState currentState = EnemyState.Idle;
        protected Transform player;
        protected Vector2 startPosition;
        protected Vector2 wanderTarget;
        protected float stateTimer;
        protected float lastAttackTime;
        protected bool facingRight = true;
        
        public System.Action<Enemy> OnEnemyDeath;
        
        protected virtual void Awake()
        {
            // Get components
            if (enemyRigidbody == null)
                enemyRigidbody = GetComponent<Rigidbody2D>();
            if (enemyAnimator == null)
                enemyAnimator = GetComponent<Animator>();
            if (enemySpriteRenderer == null)
                enemySpriteRenderer = GetComponent<SpriteRenderer>();
            if (enemyCollider == null)
                enemyCollider = GetComponent<Collider2D>();
                
            abilityController = GetComponent<AbilityController>();
            statusEffectController = GetComponent<StatusEffectController>();
            contactDamage = GetComponent<DamageDealer>();
            
            // Set detection point if not assigned
            if (detectionPoint == null)
                detectionPoint = transform;
                
            // Store starting position
            startPosition = transform.position;
            
            // Find player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        protected virtual void Start()
        {
            // Initialize with enemy data
            if (enemyData != null)
            {
                InitializeFromData();
            }
        }
        
        protected virtual void Update()
        {
            if (statusEffectController != null && statusEffectController.IsMovementImpaired())
            {
                // Don't update AI if movement is impaired
                return;
            }
            
            UpdateAI();
            UpdateAnimations();
        }
        
        protected virtual void InitializeFromData()
        {
            if (enemyData == null) return;
            
            // Set health
            m_maxHealth = enemyData.maxHealth;
            m_currentHealth = m_maxHealth;
            
            // Set sprite
            if (enemySpriteRenderer != null && enemyData.enemySprite != null)
            {
                enemySpriteRenderer.sprite = enemyData.enemySprite;
            }
            
            // Setup contact damage
            if (contactDamage != null)
            {
                contactDamage.SetDamage(enemyData.contactDamage);
            }
            
            // Setup abilities
            if (abilityController != null && enemyData.abilities != null)
            {
                foreach (var ability in enemyData.abilities)
                {
                    abilityController.AddAbility(ability);
                }
            }
        }
        
        protected virtual void UpdateAI()
        {
            stateTimer += Time.deltaTime;
            
            switch (currentState)
            {
                case EnemyState.Idle:
                    HandleIdleState();
                    break;
                case EnemyState.Wandering:
                    HandleWanderingState();
                    break;
                case EnemyState.Chasing:
                    HandleChasingState();
                    break;
                case EnemyState.Attacking:
                    HandleAttackingState();
                    break;
                case EnemyState.Retreating:
                    HandleRetreatingState();
                    break;
            }
            
            // Check for player detection
            CheckPlayerDetection();
        }
        
        protected virtual void HandleIdleState()
        {
            if (stateTimer >= enemyData.wanderTimer)
            {
                ChangeState(EnemyState.Wandering);
            }
        }
        
        protected virtual void HandleWanderingState()
        {
            if (Vector2.Distance(transform.position, wanderTarget) < 0.5f || stateTimer >= enemyData.wanderTimer * 2)
            {
                ChangeState(EnemyState.Idle);
                return;
            }
            
            MoveTowards(wanderTarget);
        }
        
        protected virtual void HandleChasingState()
        {
            if (player == null)
            {
                ChangeState(EnemyState.Idle);
                return;
            }
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
            if (distanceToPlayer > enemyData.detectionRange * 1.5f)
            {
                // Lost player, return to wandering
                ChangeState(EnemyState.Wandering);
                return;
            }
            
            if (distanceToPlayer <= enemyData.attackRange && Time.time >= lastAttackTime + enemyData.attackCooldown)
            {
                ChangeState(EnemyState.Attacking);
                return;
            }
            
            MoveTowards(player.position);
        }
        
        protected virtual void HandleAttackingState()
        {
            if (player == null)
            {
                ChangeState(EnemyState.Idle);
                return;
            }
            
            // Try to use abilities if available
            if (abilityController != null)
            {
                var abilities = abilityController.GetAbilities();
                for (int i = 0; i < abilities.Length; i++)
                {
                    if (!abilityController.IsAbilityOnCooldown(i))
                    {
                        abilityController.TryUseAbility(i, player.position);
                        lastAttackTime = Time.time;
                        break;
                    }
                }
            }
            
            lastAttackTime = Time.time;
            
            // Determine next state based on behavior type
            if (enemyData.behaviorType == EnemyBehaviorType.Coward)
            {
                ChangeState(EnemyState.Retreating);
            }
            else
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        
        protected virtual void HandleRetreatingState()
        {
            if (player == null)
            {
                ChangeState(EnemyState.Idle);
                return;
            }
            
            Vector2 retreatDirection = (transform.position - player.position).normalized;
            Vector2 retreatTarget = (Vector2)transform.position + retreatDirection * enemyData.retreatDistance;
            
            MoveTowards(retreatTarget);
            
            if (Vector2.Distance(transform.position, player.position) >= enemyData.detectionRange)
            {
                ChangeState(EnemyState.Idle);
            }
        }
        
        protected virtual void CheckPlayerDetection()
        {
            if (player == null || enemyData == null) return;
            
            float distanceToPlayer = Vector2.Distance(detectionPoint.position, player.position);
            
            if (distanceToPlayer <= enemyData.detectionRange)
            {
                switch (enemyData.behaviorType)
                {
                    case EnemyBehaviorType.Aggressive:
                        if (currentState != EnemyState.Chasing && currentState != EnemyState.Attacking)
                            ChangeState(EnemyState.Chasing);
                        break;
                        
                    case EnemyBehaviorType.Territorial:
                        float distanceFromStart = Vector2.Distance(transform.position, startPosition);
                        if (distanceFromStart <= enemyData.wanderRadius && currentState != EnemyState.Chasing && currentState != EnemyState.Attacking)
                            ChangeState(EnemyState.Chasing);
                        break;
                        
                    case EnemyBehaviorType.Coward:
                        if (currentState != EnemyState.Retreating)
                            ChangeState(EnemyState.Retreating);
                        break;
                        
                    case EnemyBehaviorType.Guardian:
                        if (distanceToPlayer <= enemyData.attackRange && currentState != EnemyState.Attacking)
                            ChangeState(EnemyState.Attacking);
                        break;
                }
            }
        }
        
        protected virtual void MoveTowards(Vector2 target)
        {
            if (enemyData == null) return;
            
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            float speed = currentState == EnemyState.Chasing ? enemyData.chaseSpeed : enemyData.moveSpeed;
            
            // Apply status effect movement modifier
            if (statusEffectController != null)
            {
                speed *= statusEffectController.GetMovementSpeedMultiplier();
            }
            
            enemyRigidbody.velocity = new Vector2(direction.x * speed, enemyRigidbody.velocity.y);
            
            // Handle facing direction
            if (direction.x > 0 && !facingRight)
                Flip();
            else if (direction.x < 0 && facingRight)
                Flip();
        }
        
        protected virtual void Flip()
        {
            facingRight = !facingRight;
            if (enemySpriteRenderer != null)
            {
                enemySpriteRenderer.flipX = !facingRight;
            }
        }
        
        protected virtual void ChangeState(EnemyState newState)
        {
            currentState = newState;
            stateTimer = 0f;
            
            // Set new wander target when entering wandering state
            if (newState == EnemyState.Wandering)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                wanderTarget = startPosition + randomDirection * enemyData.wanderRadius;
            }
        }
        
        protected virtual void UpdateAnimations()
        {
            if (enemyAnimator == null) return;
            
            enemyAnimator.SetFloat("Speed", Mathf.Abs(enemyRigidbody.velocity.x));
            enemyAnimator.SetBool("IsGrounded", true); // You might want to implement ground checking
            enemyAnimator.SetInteger("State", (int)currentState);
        }
        
        protected override void Die()
        {
            // Handle drops
            if (enemyData != null && enemyData.possibleDrops != null)
            {
                foreach (var drop in enemyData.possibleDrops)
                {
                    if (Random.value <= drop.dropChance)
                    {
                        int dropAmount = Random.Range(drop.minAmount, drop.maxAmount + 1);
                        for (int i = 0; i < dropAmount; i++)
                        {
                            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;
                            Instantiate(drop.dropPrefab, spawnPos, Quaternion.identity);
                        }
                    }
                }
            }
            
            OnEnemyDeath?.Invoke(this);
            
            base.Die();
        }
        
        public void SetEnemyData(EnemyData newData)
        {
            enemyData = newData;
            if (Application.isPlaying)
                InitializeFromData();
        }
        
        public EnemyData GetEnemyData() => enemyData;
        public EnemyState GetCurrentState() => currentState;
        
        // Gizmos for debugging
        protected virtual void OnDrawGizmosSelected()
        {
            if (enemyData == null) return;
            
            // Detection range
            Gizmos.color = Color.yellow;
            Vector3 pos = detectionPoint != null ? detectionPoint.position : transform.position;
            Gizmos.DrawWireSphere(pos, enemyData.detectionRange);
            
            // Attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, enemyData.attackRange);
            
            // Wander area
            Gizmos.color = Color.green;
            Vector3 startPos = Application.isPlaying ? (Vector3)startPosition : transform.position;
            Gizmos.DrawWireSphere(startPos, enemyData.wanderRadius);
        }
    }
    
    public enum EnemyState
    {
        Idle,
        Wandering,
        Chasing,
        Attacking,
        Retreating
    }
}