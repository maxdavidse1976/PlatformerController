using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Input : MonoBehaviour
    {
        private PlayerInput m_inputActions;

        private InputAction m_playerMovementIA;
        private InputAction m_playerJumpIA;
        
        bool _isJumping;

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
            m_inputActions = new PlayerInput();

            m_playerMovementIA = m_inputActions.Player.Move;
            m_playerMovementIA.Enable();

            m_playerJumpIA = m_inputActions.Player.Jump;
            m_playerJumpIA.Enable();

            //m_playerInteractIA = m_inputActions.Player.Interact;
            //m_playerInteractIA.Enable();
        }

        void ClearInputActions()
        {
            m_inputActions.Player.Disable();
            m_playerMovementIA.Disable();
            m_playerJumpIA.Disable();
            m_inputActions = null;
        }

        public float RetrieveMoveInput()
        {
            Vector2 movement = m_playerMovementIA.ReadValue<Vector2>();
            return movement.x;
        }

        public bool JumpPressedThisFrame()
        {
            return m_playerJumpIA.triggered;
        }
        public bool JumpEndedThisFrame()
        {
            return m_playerJumpIA.WasReleasedThisFrame();
        }
        public bool JumpPressed()
        {
            return m_playerJumpIA.IsPressed();
        }
    }
}
