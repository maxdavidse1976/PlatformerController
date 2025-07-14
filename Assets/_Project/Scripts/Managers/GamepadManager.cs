using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DragonspiritGames.PlatformerController
{
    public class GamepadManager : MonoBehaviour
    {
        [SerializeField] float _leftRumbleStrength = .1f;
        [SerializeField] float _rightRumbleStrength = 0f;
        [SerializeField] bool _rumbleEnabled = false;

        void Start()
        {
        
        }

        void Update()
        {
            if (Gamepad.current != null)
            {
                //Debug.Log(Gamepad.current?.name);
                if (_rumbleEnabled)
                {
                    Gamepad.current.SetMotorSpeeds(_leftRumbleStrength, _rightRumbleStrength);
                }
                else
                {
                    Gamepad.current.SetMotorSpeeds(0, 0);
                }
            }
        }

        
    }
}
