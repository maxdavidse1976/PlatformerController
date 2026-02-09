using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public abstract class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int currentHealth;
        [SerializeField] protected bool destroyOnDeath;

        public virtual int GetMaxHealthValue() { return maxHealth; }
        public virtual int GetCurrentHealthValue() { return currentHealth; }

        public virtual void TakeDamage(int _damageAmount)
        {
            currentHealth -= _damageAmount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void AddHealth(int healAmount)
        {
            currentHealth += healAmount;
            if (currentHealth > GetMaxHealthValue())
            {
                currentHealth = GetMaxHealthValue();
            }
        }

        protected virtual void Die()
        {
            if (destroyOnDeath) Destroy(gameObject);
            else gameObject.SetActive(false);
        }

        private void ResetHealth()
        {
            currentHealth = maxHealth;
        }

        protected void OnEnable()
        {
            ResetHealth();
        }
    }
}
