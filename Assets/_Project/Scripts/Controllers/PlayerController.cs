using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        PlayerInput _inputActions;
        bool _isJumping;

        void OnEnable()
        {
            _inputActions = new PlayerInput();
            _inputActions.Player.Enable();
            _inputActions.Player.Jump.started += JumpStarted;
            _inputActions.Player.Jump.canceled += JumpCanceled;
        }

        void OnDisable()
        {
            _inputActions.Player.Disable();
            _inputActions.Player.Jump.started -= JumpStarted;
            _inputActions.Player.Jump.canceled -= JumpCanceled;
            _inputActions = null;
        }

        void JumpStarted(InputAction.CallbackContext context)
        {
            _isJumping = true;
        }

        void JumpCanceled(InputAction.CallbackContext context)
        {
            _isJumping = false;
        }

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return _isJumping;
        }

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            return _inputActions.Player.Move.ReadValue<Vector2>().x;
        }

    }
}
