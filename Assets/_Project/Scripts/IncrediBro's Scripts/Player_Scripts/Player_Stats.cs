using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Stats : Health
    {
        public static Player_Stats Instance;
        [SerializeField] private bool m_dontDestroyOnLoad;
        [Space(10)]

        [Header("Components")]
        public Rigidbody2D M_Rigidbody;
        public Animator M_Animator;
        public Player_Input M_Input;
        public SpriteRenderer M_SpriteRenderer;
        public Transform M_HoldThrowTransform;

        [Space(10)]

        [Header("Physics")]
        public bool M_OnGround;
        public bool M_OnLadder;
        public float M_Friction = 0f;

        private void Awake()
        {
            if(Instance== null) Instance = this;

            if(Instance!= null && Instance!= this)
            {
                Destroy(this.gameObject);
            }

            if (m_dontDestroyOnLoad)
            {
                m_destroyOnDeath = false;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
