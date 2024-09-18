using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public bool MenuOpenCloseInput {  get; private set; }

        PlayerInput _playerInput;

        InputAction _menuOpenCloseAction;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

        }

        //void Update()
        //{
        //    MenuOpenCloseInput = _menuOpenCloseAction.WasPerformedThisFrame();
        //}
    }
}
