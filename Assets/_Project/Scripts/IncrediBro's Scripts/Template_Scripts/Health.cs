using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public abstract class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] protected int m_maxHealth;
        [SerializeField] protected int m_currentHealth;
        [SerializeField] protected bool m_destroyOnDeath;

        public virtual int GetMaxHealthValue() { return m_maxHealth; }
        public virtual int GetCurrentHealthValue() { return m_currentHealth; }

        public virtual void TakeDamage(int _damageAmount)
        {
            m_currentHealth -= _damageAmount;
            if (m_currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void AddHealth(int _healAmount)
        {
            m_currentHealth += _healAmount;
            if (m_currentHealth > GetMaxHealthValue())
            {
                m_currentHealth = GetMaxHealthValue();
            }
        }

        protected virtual void Die()
        {
            if (m_destroyOnDeath) Destroy(gameObject);
            else gameObject.SetActive(false);
        }

        private void ResetHealth()
        {
            m_currentHealth = m_maxHealth;
        }

        protected void OnEnable()
        {
            ResetHealth();
        }
    }
}
