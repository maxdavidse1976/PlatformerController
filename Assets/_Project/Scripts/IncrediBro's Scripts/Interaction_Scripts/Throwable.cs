using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Throwable : Interactable
    {
        private bool m_isHeld;
        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void Interact()
        {
            if (!m_isHeld)
            {
                Transform _target = Player_Stats.Instance.M_HoldThrowTransform;

                transform.position = _target.position;
                transform.SetParent(_target);
                m_rigidbody.velocity = Vector2.zero;
                m_rigidbody.isKinematic = true;
                m_isHeld = true;
            }
            else
            {
                m_rigidbody.isKinematic = false;
                //m_rigidbody.AddForce()
                m_isHeld = false;
            }
        }
    }
}
