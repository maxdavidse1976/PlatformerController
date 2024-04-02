using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Stats : MonoBehaviour
    {
        [Header("Components")]
        public Rigidbody2D M_Rigidbody;
        public Animator M_Animator;
        public Player_Input M_Input;
        public SpriteRenderer M_SpriteRenderer;

        [Space(10)]

        [Header("Physics")]
        public bool M_Grounded;
        public float M_Friction = 0f;
        
    }
}
