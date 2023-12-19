using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace DragonspiritGames.PlatformerController
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int _maximumHealth = 100;
        [SerializeField] int _currentHealth;

        public static Health Instance;

        void Awake()
        {
            _currentHealth = _maximumHealth;
            Debug.Log(_currentHealth);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Instance = Instance.GetComponent<Health>();
            }
        }

        public void HealPlayer(int healthAdded)
        {
            _currentHealth += healthAdded;
            if (_currentHealth > _maximumHealth) 
                _currentHealth = _maximumHealth;
        }
    }
}
