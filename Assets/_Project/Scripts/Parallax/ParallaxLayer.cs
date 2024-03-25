using System.Security.Cryptography;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class ParallaxLayer : MonoBehaviour
    {
        [Range(-1f, 1f)] [SerializeField] float _parallaxAmount;
        [System.NonSerialized] public Vector3 newPosition;
        bool _adjusted = false;

        public void MoveLayer(float positionChangeX, float positionChangeY)
        {
            newPosition = transform.localPosition;
            newPosition.x -= positionChangeX * (-_parallaxAmount * 40) * (Time.deltaTime);
            newPosition.y -= positionChangeY * (-_parallaxAmount * 40) * (Time.deltaTime);
            transform.localPosition = newPosition;
        }
    }
}
