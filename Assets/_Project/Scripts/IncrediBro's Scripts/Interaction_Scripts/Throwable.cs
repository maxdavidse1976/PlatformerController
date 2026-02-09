using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Throwable : Interactable
    {
        [SerializeField] float _radius;
        [SerializeField] LayerMask _groundLayer;

        bool _isHeld;
        bool _canThrow;

        Rigidbody2D _rigidbody;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        bool IsGrounded()
        {
            Collider2D _hit = Physics2D.OverlapCircle(transform.position, _radius, _groundLayer);

            if (_hit) return true;

            return false;
        }


        void Pickup()
        {
            Transform _target = Player_Stats.Instance.HoldThrowTransform;

            transform.position = _target.position;
            transform.SetParent(_target);
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody.velocity = Vector2.zero;
            _isHeld = true;
            StartCoroutine(WaitForSecond(.2f));
        }

        IEnumerator WaitForSecond(float _time)
        {
            yield return new WaitForSeconds(_time);
            _canThrow = true;
        }

        public void Throw()
        {
            if (_canThrow)
            {
                _rigidbody.transform.parent = null;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
                _rigidbody.AddForce(new Vector2(Player_Stats.Instance.transform.localScale.x * Player_Stats.Instance.ThrowForce, 1f), ForceMode2D.Impulse);
                _isHeld = false;
                _canThrow = false;
            }
        }

        public override void Interact()
        {
            if (!_isHeld)
            {
                Pickup();
            }
        }

    }
}
