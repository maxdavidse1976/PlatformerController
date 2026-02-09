using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Input : MonoBehaviour
    {
        PlayerInput _inputActions;

        InputAction _playerMovementIA;
        InputAction _playerJumpIA;
        InputAction _playerInteractIA;

        void OnEnable()
        {
            InitializeInputActions();
        }

        void OnDisable()
        {
            ClearInputActions();
        }

        void InitializeInputActions()
        {
            _inputActions = new PlayerInput();

            _playerMovementIA = _inputActions.Player.Move;
            _playerMovementIA.Enable();

            _playerJumpIA = _inputActions.Player.Jump;
            _playerJumpIA.Enable();

            _playerInteractIA = _inputActions.Player.Interact;
            _playerInteractIA.Enable();
        }

        void ClearInputActions()
        {
            _inputActions.Player.Disable();
            _playerMovementIA.Disable();
            _playerJumpIA.Disable();
            _inputActions = null;
        }

        public float GetHorizontalAxis()
        {
            Vector2 movement = _playerMovementIA.ReadValue<Vector2>();
            return movement.x;
        }

        public float GetVerticalAxis()
        {
            Vector2 movement = _playerMovementIA.ReadValue<Vector2>();
            return movement.y;
        }

        public bool JumpPressedThisFrame()
        {
            return _playerJumpIA.triggered;
        }
        public bool JumpEndedThisFrame()
        {
            return _playerJumpIA.WasReleasedThisFrame();
        }
        public bool JumpPressed()
        {
            return _playerJumpIA.IsPressed();
        }

        public bool InteractPressedThisFrame()
        {
            return _playerInteractIA.triggered;
        }
        public bool InteractEndedThisFrame()
        {
            return _playerInteractIA.WasReleasedThisFrame();
        }
        public bool InteractPressed()
        {
            return _playerInteractIA.IsPressed();
        }
    }
}
