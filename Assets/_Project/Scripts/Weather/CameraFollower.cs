using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] bool _followX = true;
        [SerializeField] bool _followY = true;
        [SerializeField] bool _followZ = false;
        [SerializeField] Vector3 _offset;

        void LateUpdate()
        {
            if (_target == null) return;

            Vector3 position = transform.position;
            Vector3 targetPosition = _target.position + _offset;

            if (_followX) position.x = targetPosition.x;
            if (_followY) position.y = targetPosition.y;
            if (_followZ) position.z = targetPosition.z;

            transform.position = position;
        }
    }
}
