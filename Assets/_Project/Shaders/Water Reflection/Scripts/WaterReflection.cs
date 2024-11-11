using UnityEngine;

public class WaterReflection : MonoBehaviour
{
    [Tooltip("The texture that we use to generate the water.")]
    [SerializeField]
    Texture waterTexture;
    
    [Tooltip("Sprite that will receive the water shader.")]
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [Tooltip("Camera used to capture the reflection scene.")]
    [SerializeField] 
    Camera camera;

    [Tooltip("Resolution of the reflection's texture.")]
    [SerializeField] 
    int pixelsPerUnit = 32;

    [Tooltip("The color of the water")]
    [SerializeField] 
    Color color;

    [Tooltip("How much is the reflection squeezed vertically. 1 : no squeeze, > 1 : smaller reflection, < 1 : taller reflection.")]
    [SerializeField] 
    float verticalSqueezeRatio = 1;

    [Tooltip("By default, the camera used to capture the reflection's scene is placed just above the sprite renderer. You can adjust camera height by modifying this offset.")]
    [SerializeField] 
    float verticalCameraOffset = 0;

    [Header("======== Shader parameters ========")]

    [Tooltip("Shader used to simulate water")]
    [SerializeField]
    Shader waterShader;

    [Tooltip("Strength of the water's turbulence")]
    [SerializeField]
    float turbulencesStrength = 0.4f;

    [Tooltip("Speed of the water")]
    [SerializeField]
    float waterSpeed = 0.01f;

    [Tooltip("How much refraction (> 0) or Reflection(< 0) patterns are visible.")]
    [SerializeField]
    float refraction = 0.5f;

    [Tooltip("Scale of noise. Used to move and distord turbulences in a more realistic way.")]
    [SerializeField]
    float noiseScale = 10;

    [Tooltip("Power given to noise. Used to move and distord turbulences in a more realistic way.")]
    [SerializeField]
    float noisePower = 0.03f;

    [Tooltip("Wave patterns inversed scale.")]
    [SerializeField]
    Vector2 waveInversedScale = Vector2.one;

    RenderTexture _renderTexture;
    Material _waterMaterial;

    public void Awake()
    {
        // Unity modify camera's aspect when game starts (according to your screen dimensions). So we call UpdateCamera here to override it.
        UpdateCamera();
    }

    #region Get Methods

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
    #endregion

    #region Set Methods
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

    public void UpdateCamera()
    {
        if (spriteRenderer != null && camera != null && waterShader != null)
        {
            _renderTexture = new RenderTexture((int)spriteRenderer.bounds.size.x * pixelsPerUnit, (int)spriteRenderer.bounds.size.y * pixelsPerUnit, 1);
            _renderTexture.name = "Render Texture";

            float cameraHeight = spriteRenderer.bounds.size.y * verticalSqueezeRatio;
            camera.aspect = spriteRenderer.bounds.size.x / cameraHeight;
            camera.orthographicSize = verticalSqueezeRatio * spriteRenderer.bounds.size.y / 2;
            camera.targetTexture = _renderTexture;
            Vector3 cameraPosition = camera.transform.position;
            cameraPosition.x = spriteRenderer.transform.position.x;
            cameraPosition.y = verticalCameraOffset + spriteRenderer.transform.position.y + spriteRenderer.bounds.size.y / 2 + cameraHeight / 2;
            camera.transform.position = cameraPosition;

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
        }
    }

}
