using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaterReflection : MonoBehaviour
{
    [Tooltip("The texture that we use to generate the water.")]
    [SerializeField] private Texture waterTexture;

    [Tooltip("Sprite that will receive the water shader.")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Tooltip("Camera used to capture the reflection scene.")]
    [SerializeField] private Camera camera;

    [Tooltip("Resolution of the reflection's texture.")]
    [SerializeField] private int pixelsPerUnit = 32;

    [Tooltip("The color of the water")]
    [SerializeField] private Color color;

    [Tooltip("How much is the reflection squeezed vertically. 1 : no squeeze, > 1 : smaller reflection, < 1 : taller reflection.")]
    [SerializeField] private float verticalSqueezeRatio = 1;

    [Tooltip("By default, the camera used to capture the reflection's scene is placed just above the sprite renderer. You can adjust camera height by modifying this offset.")]
    [SerializeField] private float verticalCameraOffset = 0;

    [Header("======== Shader parameters ========")]

    [Tooltip("Shader used to simulate water")]
    [SerializeField] private Shader waterShader;

    [Tooltip("Strength of the water's turbulence")]
    [SerializeField] private float turbulencesStrength = 0.4f;

    [Tooltip("Speed of the water")]
    [SerializeField] private float waterSpeed = 0.01f;

    [Tooltip("How much refraction (> 0) or Reflection(< 0) patterns are visible.")]
    [SerializeField] private float refraction = 0.5f;

    [Tooltip("Scale of noise. Used to move and distord turbulences in a more realistic way.")]
    [SerializeField] private float noiseScale = 10;

    [Tooltip("Power given to noise. Used to move and distord turbulences in a more realistic way.")]
    [SerializeField] private float noisePower = 0.03f;

    [Tooltip("Wave patterns inversed scale.")]
    [SerializeField] private Vector2 waveInversedScale = Vector2.one;

    private RenderTexture _renderTexture;
    private Material _waterMaterial;

    // Track previous positions for change detection
    private Vector3 _lastSpritePosition;
    private Vector3 _lastCameraPosition;
    private Vector3 _lastSpriteScale;

    private void Awake()
    {
        CacheTransformData();
    }

    private void Start()
    {
        // Use LateStart pattern to ensure all objects are properly initialized
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        // Wait until the end of the frame to ensure all objects are initialized
        yield return new WaitForEndOfFrame();
        UpdateCamera();
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Only run in edit mode for editor updates
        if (!Application.isPlaying)
        {
            CheckForTransformChanges();
        }
    }

    private void OnValidate()
    {
        // Called when inspector values change
        if (Application.isPlaying)
        {
            UpdateCamera();
        }
        else
        {
            // In edit mode, delay the update to next frame
            EditorApplication.delayCall += UpdateCamera;
        }
    }
#endif

    private void CheckForTransformChanges()
    {
        if (spriteRenderer == null || camera == null) return;

        bool hasChanged = false;

        // Check if sprite renderer has moved or scaled
        if (_lastSpritePosition != spriteRenderer.transform.position ||
            _lastSpriteScale != spriteRenderer.transform.localScale)
        {
            hasChanged = true;
        }

        // Check if camera has moved
        if (_lastCameraPosition != camera.transform.position)
        {
            hasChanged = true;
        }

        if (hasChanged)
        {
            UpdateCamera();
            CacheTransformData();
        }
    }

    private void CacheTransformData()
    {
        if (spriteRenderer != null)
        {
            _lastSpritePosition = spriteRenderer.transform.position;
            _lastSpriteScale = spriteRenderer.transform.localScale;
        }

        if (camera != null)
        {
            _lastCameraPosition = camera.transform.position;
        }
    }

    public void UpdateCamera()
    {
        if (spriteRenderer != null && camera != null && waterShader != null)
        {
            // Clean up old render texture
            if (_renderTexture != null)
            {
                if (Application.isPlaying)
                    Destroy(_renderTexture);
                else
                    DestroyImmediate(_renderTexture);
            }

            _renderTexture = new RenderTexture(
                (int)(spriteRenderer.bounds.size.x * pixelsPerUnit),
                (int)(spriteRenderer.bounds.size.y * pixelsPerUnit),
                1);
            _renderTexture.name = "Water Reflection Render Texture";

            float cameraHeight = spriteRenderer.bounds.size.y * verticalSqueezeRatio;
            camera.aspect = spriteRenderer.bounds.size.x / cameraHeight;
            camera.orthographicSize = verticalSqueezeRatio * spriteRenderer.bounds.size.y / 2;
            camera.targetTexture = _renderTexture;

            Vector3 cameraPosition = camera.transform.position;
            cameraPosition.x = spriteRenderer.transform.position.x;
            cameraPosition.y = verticalCameraOffset + spriteRenderer.transform.position.y +
                              spriteRenderer.bounds.size.y / 2 + cameraHeight / 2;
            camera.transform.position = cameraPosition;

            // Clean up old material
            if (_waterMaterial != null)
            {
                if (Application.isPlaying)
                    Destroy(_waterMaterial);
                else
                    DestroyImmediate(_waterMaterial);
            }

            _waterMaterial = new Material(waterShader);
            _waterMaterial.SetTexture("_WaterTex", waterTexture);
            _waterMaterial.SetTexture("_RenderTex", _renderTexture);
            _waterMaterial.SetColor("_Color", color);
            _waterMaterial.SetFloat("_TurbulencesStrength", turbulencesStrength);
            _waterMaterial.SetFloat("_WaterSpeed", waterSpeed);
            _waterMaterial.SetFloat("_Refraction", refraction);
            _waterMaterial.SetFloat("_NoiseScale", noiseScale);
            _waterMaterial.SetFloat("_NoisePower", noisePower);
            _waterMaterial.SetVector("_PatternSizeReduction", waveInversedScale);

            spriteRenderer.material = _waterMaterial;

            CacheTransformData();
        }
    }

    private void OnDestroy()
    {
        // Clean up resources
        if (_renderTexture != null)
        {
            if (Application.isPlaying)
                Destroy(_renderTexture);
            else
                DestroyImmediate(_renderTexture);
        }

        if (_waterMaterial != null)
        {
            if (Application.isPlaying)
                Destroy(_waterMaterial);
            else
                DestroyImmediate(_waterMaterial);
        }
    }

    #region Get/Set Methods (unchanged)
    public Color GetColor() => color;
    public float GetNoisePower() => noisePower;
    public float GetNoiseScale() => noiseScale;
    public int GetPixelsPerUnit() => pixelsPerUnit;
    public float GetRefraction() => refraction;
    public float GetTurbulenceStrength() => turbulencesStrength;
    public float GetVerticalCameraOffset() => verticalCameraOffset;
    public float GetVerticalSqueezeRatio() => verticalSqueezeRatio;
    public float GetWaterSpeed() => waterSpeed;
    public Vector2 GetWaveInversedScale() => waveInversedScale;

    public void SetColor(Color newColor) { color = newColor; }
    public void SetNoisePower(float newNoisePower) { noisePower = newNoisePower; }
    public void SetNoiseScale(float newNoiseScale) { noiseScale = newNoiseScale; }
    public void SetPixelsPerUnit(int newPixelsPerUnit) { pixelsPerUnit = newPixelsPerUnit; }
    public void SetRefraction(float newRefraction) { refraction = newRefraction; }
    public void SetTurbulenceStrength(float newTurbulenceStrength) { turbulencesStrength = newTurbulenceStrength; }
    public void SetVerticalCameraOffset(float newVerticalCameraOffSet) { verticalCameraOffset = newVerticalCameraOffSet; }
    public void SetVerticalSqueezeRatio(float newVerticalSqueezeRatio) { verticalSqueezeRatio = newVerticalSqueezeRatio; }
    public void SetWaterSpeed(float newWaterSpeed) { waterSpeed = newWaterSpeed; }
    public void SetWaveInversedScale(Vector2 newWaveInversedScale) { waveInversedScale = newWaveInversedScale; }
    #endregion
}
