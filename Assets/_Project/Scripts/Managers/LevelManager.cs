using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DragonspiritGames.PlatformerController
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ChangeLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
