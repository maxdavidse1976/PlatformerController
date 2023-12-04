using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [RequireComponent(typeof(Controller), typeof(Rigidbody2D), typeof(CollisionDataDetector))]
    public class Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] float _jumpHeight = 3f;
        [SerializeField, Range(0f, 5f)] int _maxAirJumps = 1;
        [SerializeField, Range(0f, 5f)] float _downwardGravity = 3f;
        [SerializeField, Range(0f, 5f)] float _upwardGravity = 1.7f;
        [SerializeField, Range(0f, 0.3f)] float _coyoteTime = 0.2f;
        [SerializeField, Range(0f, 0.3f)] float _jumpBufferTime = 0.2f;

        Controller _controller;
        Rigidbody2D _rigidbody;
        CollisionDataDetector _collisionDetector;
        Vector2 _velocity;

        float _defaultGravity, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;
        bool _onGround, _isJumping, _desiredJump, _isJumpReset;
        int _jumpPhase;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collisionDetector = GetComponent<CollisionDataDetector>();
            _controller = GetComponent<Controller>();

            _isJumpReset = true;
            _defaultGravity = 1f;
        }

        void Update()
        {
            _desiredJump = _controller.input.RetrieveJumpInput(this.gameObject);
        }

        void FixedUpdate()
        {
            _onGround = _collisionDetector.OnGround;
            _velocity = _rigidbody.velocity;

            if (_onGround & _rigidbody.velocity.y == 0 ) 
            {
                _jumpPhase = 0;
                _coyoteCounter = _coyoteTime;
                _isJumping = false;
            }
            else
            {
                _coyoteCounter -= Time.deltaTime;
            }

            if (_desiredJump && _isJumpReset)
            {
                _isJumpReset = false;
                _desiredJump = false;
                _jumpBufferCounter = _jumpBufferTime;
            }
            else if (_jumpBufferCounter > 0)
            {
                _jumpBufferCounter -= Time.deltaTime;
            }
            else if (!_desiredJump)
            {
                _isJumpReset = true;
            }
            if (_jumpBufferCounter > 0)
            {
                JumpAction();
            }

            if (_controller.input.RetrieveJumpInput(this.gameObject) && _rigidbody.velocity.y > 0f)
            {
                _rigidbody.gravityScale = _upwardGravity;
            }
            else if (!_controller.input.RetrieveJumpInput(this.gameObject) && _rigidbody.velocity.y < 0f)
            {
                _rigidbody.gravityScale = _downwardGravity;
            }
            else if (_rigidbody.velocity.y == 0f)
            {
                _rigidbody.gravityScale = _defaultGravity;
            }

            _rigidbody.velocity = _velocity;
        }

        void JumpAction()
        {
            if (_coyoteCounter > 0f || (_jumpPhase < _maxAirJumps && _isJumping))
            {
                if (_isJumping)
                {
                    _jumpPhase++;
                }
                _jumpBufferCounter = 0;
                _coyoteCounter = 0;
                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardGravity);
                _isJumping = true;

                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                if (_velocity.y < 0f)
                {
                    _jumpSpeed = Mathf.Abs(_rigidbody.velocity.y);
                }
                _velocity.y = _jumpSpeed;
            }
        }
    }
}
