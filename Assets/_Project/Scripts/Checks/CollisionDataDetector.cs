using System;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class CollisionDataDetector : MonoBehaviour
    {
        public bool OnGround { get; private set; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get; private set; }

        PhysicsMaterial2D _material;

        void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            OnGround = false;
            Friction = 0f;
        }

        void EvaluateCollision(Collision2D collision)
        {
            for (int i=0; i < collision.contactCount; i++)
            {
                ContactNormal = collision.GetContact(i).normal;
                OnGround |= ContactNormal.y >= 0.9f;
            }

        }

        void RetrieveFriction(Collision2D collision)
        {
            _material = collision.rigidbody.sharedMaterial;

            Friction = 0f;
            if (_material != null)
            {
                Friction = _material.friction;
            }
        }
    }
}
