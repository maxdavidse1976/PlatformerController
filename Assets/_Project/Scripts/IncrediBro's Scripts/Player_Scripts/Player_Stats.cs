using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Player_Stats : Health
    {
        public static Player_Stats Instance;

        [SerializeField] bool _dontDestroyOnLoad;
        [Space(10)]

        [Header("Components")]
        public Rigidbody2D Rigidbody;
        public Animator Animator;
        public Player_Input Input;
        public SpriteRenderer SpriteRenderer;
        public Transform HoldThrowTransform;

        [Space(10)]

        [Header("Physics")]
        public bool OnGround;
        public bool OnLadder;
        public float Friction = 0f;
        public float ThrowForce = 10f;

        void Awake()
        {
            if(Instance== null) Instance = this;

            if(Instance!= null && Instance!= this)
            {
                Destroy(this.gameObject);
            }

            if (_dontDestroyOnLoad)
            {
                destroyOnDeath = false;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
