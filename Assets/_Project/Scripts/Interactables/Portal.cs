using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DragonspiritGames.PlatformerController
{
    public class Portal : Interactable
    {
        [SerializeField] string _levelName;
        public override void Interact()
        {
            LevelManager.Instance.ChangeLevel(_levelName);
        }
    }
}
