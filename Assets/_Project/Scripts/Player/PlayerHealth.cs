using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace DragonspiritGames.PlatformerController
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] int _maximumHealth = 100;
        [SerializeField] int _currentHealth;

        public static PlayerHealth Instance;

        void Awake()
        {
            _currentHealth = _maximumHealth;
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Instance = Instance.GetComponent<PlayerHealth>();
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
