using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaterReflection))]
public class WaterReflectionEditor : Editor
{
    WaterReflection waterReflection;
    SerializedProperty spriteRenderer;
    SerializedProperty camera;
    SerializedProperty waterShader;
    SerializedProperty waterTexture;

    void OnEnable()
    {
        waterReflection = target as WaterReflection;

        waterReflection.UpdateCamera();

        spriteRenderer = serializedObject.FindProperty("spriteRenderer");
        camera = serializedObject.FindProperty("camera");
        waterShader = serializedObject.FindProperty("waterShader");
        waterTexture = serializedObject.FindProperty("waterTexture");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(camera, new GUIContent("Camera", "Camera used to capture the reflection scene."));
        if (camera.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You must assign the camera.", MessageType.Error);
        }
        else
        {
            waterReflection.SetVerticalCameraOffset(EditorGUILayout.FloatField(new GUIContent("Vertical Camera Offset", "By default, the camera used to capture the reflection's scene is placed just above the sprite renderer. You can adjust camera height by modifying this offset."), waterReflection.GetVerticalCameraOffset()));
        }

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(spriteRenderer, new GUIContent("Sprite Renderer", "Sprite that will receive the water shader."));
        if (spriteRenderer.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("You must assign the spriteRenderer.", MessageType.Error);
        }
        else
        {
            waterReflection.SetPixelsPerUnit(EditorGUILayout.IntField(new GUIContent("Pixels Per Unit", "Resolution of the reflection's texture."), waterReflection.GetPixelsPerUnit()));
            waterReflection.SetVerticalSqueezeRatio(EditorGUILayout.FloatField(new GUIContent("Vertical Squeeze Ratio", "How much is the reflection squeezed vertically. 1 : no squeeze, > 1 : smaller reflection, < 1 : taller reflection."), waterReflection.GetVerticalSqueezeRatio()));
        }

        GUILayout.Space(10);
        GUILayout.Label("Shader Parameters", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(waterTexture, new GUIContent("Water Texture", "Water texture used to simulate water."));
        EditorGUILayout.PropertyField(waterShader, new GUIContent("Water Shader", "Shader used to simulate water."));
        waterReflection.SetColor(EditorGUILayout.ColorField(new GUIContent("Water Color", "Water's color."), waterReflection.GetColor()));
        waterReflection.SetTurbulenceStrength(EditorGUILayout.FloatField(new GUIContent("Turbulences Strength", "Strength of water's turbulences."), waterReflection.GetTurbulenceStrength()));
        waterReflection.SetWaterSpeed(EditorGUILayout.FloatField(new GUIContent("Water Speed", "Water's speed."), waterReflection.GetWaterSpeed()));
        waterReflection.SetRefraction(EditorGUILayout.FloatField(new GUIContent("Refraction/Reflection", "How much refraction (> 0) or Reflection(< 0) patterns are visible."), waterReflection.GetRefraction()));
        waterReflection.SetNoiseScale(EditorGUILayout.FloatField(new GUIContent("Noise Scale", "Scale of noise. Used to move and distord turbulences in a more realistic way."), waterReflection.GetNoiseScale()));
        waterReflection.SetNoisePower(EditorGUILayout.FloatField(new GUIContent("Noise Power", "Power given to noise. Used to move and distord turbulences in a more realistic way."), waterReflection.GetNoisePower()));
        waterReflection.SetWaveInversedScale(EditorGUILayout.Vector2Field(new GUIContent("Pattern Size Reduction", "Wave patterns inversed scale."), waterReflection.GetWaveInversedScale()));
        EditorGUI.indentLevel--;

        bool updateCamera = GUILayout.Button(new GUIContent("Force Visual Update", "If for some reason parameters are not applied, you can force them by clicking this button."));
        if (updateCamera)
        {
            waterReflection.UpdateCamera();
        }
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(waterReflection);
            waterReflection.UpdateCamera();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
