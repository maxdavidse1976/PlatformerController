using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [RequireComponent(typeof(Controller), typeof(Rigidbody2D), typeof(CollisionDataDetector))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] float _maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] float _maxAirAcceleration = 20f;

        Controller _controller;
        CollisionDataDetector _collisionDetector;
        Animator _gaiaAnimator;
        Vector2 _direction, _desiredVelocity, _velocity;
        Rigidbody2D _rigidbody;
        SpriteRenderer _spriteRenderer;

        float _acceleration, _maxSpeedChange;
        bool _onGround;


        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            _collisionDetector = GetComponent<CollisionDataDetector>();
            _gaiaAnimator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDetector.Friction, 0f);
        }

        void FixedUpdate()
        {
            _onGround = _collisionDetector.OnGround;
            _velocity = _rigidbody.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            if (Mathf.Abs(_velocity.x) > Mathf.Epsilon)
            {
                _gaiaAnimator.SetBool("isRunning", true);
            }
            else
            {
                _gaiaAnimator.SetBool("isRunning", false);
            }

            if (_velocity.x < -0.2f)
            {
                _spriteRenderer.flipX = true;
            }
            else if (_velocity.x > 0.2f)
            {
                _spriteRenderer.flipX = false;
            }

            _rigidbody.velocity = _velocity;
        }
    }
}
