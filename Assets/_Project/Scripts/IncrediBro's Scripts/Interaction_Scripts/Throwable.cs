using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class Throwable : Interactable
    {
        [SerializeField] private float m_radius;
        [SerializeField] private LayerMask m_groundLayer;

        private bool m_isHeld;
        private bool m_canThrow;

        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private bool IsGrounded()
        {
            Collider2D _hit = Physics2D.OverlapCircle(transform.position, m_radius, m_groundLayer);

            if (_hit) return true;

            return false;
        }

        public override void Interact()
        {
            if (!m_isHeld)
            {
                Pickup();
            }
        }

        private void Pickup()
        {
            Transform _target = Player_Stats.Instance.M_HoldThrowTransform;

            transform.position = _target.position;
            transform.SetParent(_target);
            m_rigidbody.bodyType = RigidbodyType2D.Kinematic;
            m_rigidbody.velocity = Vector2.zero;
            m_isHeld = true;
            StartCoroutine(WaitForSecond(.2f));
        }

        public void Throw()
        {
            if (m_canThrow)
            {
                m_rigidbody.transform.parent = null;
                m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
                m_rigidbody.AddForce(new Vector2(Player_Stats.Instance.transform.localScale.x * Player_Stats.Instance.M_ThrowForce, 1f), ForceMode2D.Impulse);
                m_isHeld = false;
                m_canThrow = false;
            }
        }

        IEnumerator WaitForSecond(float _time)
        {
            yield return new WaitForSeconds(_time);
            m_canThrow = true;
        }
    }
}
