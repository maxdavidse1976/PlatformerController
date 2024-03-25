using System;
using System.Collections.Generic;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class ParallaxController : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float cameraPositionChangeX, float cameraPositionChangeY);

        public ParallaxCameraDelegate onCameraMove;
        Vector2 _oldCameraPosition;
        List<ParallaxLayer> _parallaxLayers = new List<ParallaxLayer>();

        void Start()
        {
            onCameraMove += MoveLayer;
            FindLayer();
            _oldCameraPosition.x = Camera.main.transform.position.x;
            _oldCameraPosition.y = Camera.main.transform.position.y;
        }

        void FixedUpdate()
        {
            if (Camera.main.transform.position.x != _oldCameraPosition.x ||
                (Camera.main.transform.position.y) != _oldCameraPosition.y)
            {
                if (onCameraMove != null)
                {
                    Vector2 cameraPositionChange;
                    cameraPositionChange = new Vector2(_oldCameraPosition.x - Camera.main.transform.position.x,
                        _oldCameraPosition.y - Camera.main.transform.position.y);
                    onCameraMove(cameraPositionChange.x, cameraPositionChange.y);
                }

                _oldCameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            }
        }

        void FindLayer()
        {
            _parallaxLayers.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();
                if (layer != null)
                {
                    _parallaxLayers.Add(layer);
                }
            }
        }

        void MoveLayer(float positionChangeX, float positionChangeY)
        {
            foreach (ParallaxLayer layer in _parallaxLayers)
            {
                layer.MoveLayer(positionChangeX, positionChangeY);
            }
        }
    }
}
