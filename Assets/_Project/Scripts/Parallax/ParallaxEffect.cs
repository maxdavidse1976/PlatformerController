using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class ParallaxEffect : MonoBehaviour
    {
        [Header("General Settings")] 
        [Tooltip("Speed of the parallax effect on the X-axis. If we would set it to 0, that means no parallaxing at all, 1 means full parallax.")]
        [Range(0, 1f)]
        [SerializeField] float _parallaxSpeedX = 0.5f;

        [Tooltip("Speed of the parallax effect on the Y-axis. If we would set it to 0, that means no parallaxing at all, 1 means full parallax.")]
        [Range(0, 1f)]
        [SerializeField] float _parallaxSpeedY = 0.5f;
        
        [Header("Camera Settings")]
        [Tooltip("Reference to the camera. If we don't set this, we can still fall back to the main camera")]
        [SerializeField] Transform _camera;

        SpriteRenderer _spriteRenderer;
        Vector3 _initialCameraPosition;
        float _startPositionX;
        float _startPositionY;
        float _spriteSizeWidth;
        float _spriteSizeHeight;

        float _cameraDeltaX;
        float _cameraDeltaY;
        
        float _parallaxOffsetX;
        float _parallaxOffsetY;
        
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            SetCamera();
            StoreInitialCameraPosition();
            SetInitialSpritePosition();
            CalculateSpriteSize();
        }

        void LateUpdate()
        {
            CalculateCameraDelta();
            CalculateSpriteOffset();
            UpdateSpritePosition();
            LoopParallaxEffect();
        }
        
        void SetCamera()
        {
            if (_camera == null)
            {
                _camera = Camera.main.transform;
            }
        }

        void StoreInitialCameraPosition()
        {
            _initialCameraPosition = _camera.position;
        }
        
        void SetInitialSpritePosition()
        {
            _startPositionX = transform.position.x;
            _startPositionY = transform.position.y;
        }

        void CalculateSpriteSize()
        {
            _spriteSizeWidth = _spriteRenderer.bounds.size.x;
            _spriteSizeHeight = _spriteRenderer.bounds.size.y;
        }

        void CalculateCameraDelta()
        {
            _cameraDeltaX = _camera.position.x - _initialCameraPosition.x;
            _cameraDeltaY = _camera.position.y - _initialCameraPosition.y;
        }

        void CalculateSpriteOffset()
        {
            _parallaxOffsetX = _cameraDeltaX * _parallaxSpeedX;
            _parallaxOffsetY = _cameraDeltaY * _parallaxSpeedY;
        }

        void UpdateSpritePosition()
        {
            transform.position = new Vector3(_startPositionX + _parallaxOffsetX, _startPositionY + _parallaxOffsetY, transform.position.z);
        }

        void LoopParallaxEffect()
        {
            float relativeCameraDistX = _cameraDeltaX * (1 - _parallaxSpeedX);
            if (relativeCameraDistX > _startPositionX + _spriteSizeWidth)
            {
                _startPositionX += _spriteSizeWidth;
            }
            else if (relativeCameraDistX < _startPositionX - _spriteSizeWidth)
            {
                _startPositionX -= _spriteSizeWidth;
            }
        }
    }
}
