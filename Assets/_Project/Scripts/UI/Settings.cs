using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace DragonspiritGames.PlatformerController
{
    public class Settings : MonoBehaviour
    {
        string _settingsPath;

        Slider _volumeSlider;
        DropdownField _resolutionDropDown;
        Toggle _subtitlesToggle;
        Toggle _highContrastToggle;
        Button _saveButton;

        [System.Serializable]
        public class PlayerSettings
        {
            public float Volume = 0.1f;
            public int ScreenResolutionIndex  = 0;
            public bool SubtitlesEnabled  = false;
            public bool HighContrastEnabled = false;
        }

        PlayerSettings _currentSettings;

        void Awake()
        {
            _settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
            LoadSettings();

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _resolutionDropDown = root.Query<DropdownField>("resolutionDropdown");
            _volumeSlider = root.Query<Slider>("volumeSlider");
            _subtitlesToggle = root.Query<Toggle>("displaySubtitles");
            _highContrastToggle = root.Query<Toggle>("highContrastMode");
            _saveButton = root.Query<Button>("btnSave");

            _volumeSlider.value = _currentSettings.Volume;
            _resolutionDropDown.index = _currentSettings.ScreenResolutionIndex;
            _subtitlesToggle.value = _currentSettings.SubtitlesEnabled;
            _highContrastToggle.value = _currentSettings.HighContrastEnabled;

            _volumeSlider.RegisterValueChangedCallback(e => _currentSettings.Volume = e.newValue);
            _resolutionDropDown.RegisterValueChangedCallback(e => _currentSettings.ScreenResolutionIndex = _resolutionDropDown.index);
            _subtitlesToggle.RegisterValueChangedCallback(e => _currentSettings.SubtitlesEnabled = e.newValue);
            _highContrastToggle.RegisterValueChangedCallback(e => _currentSettings.HighContrastEnabled = e.newValue);
            _saveButton.clicked += SaveSettings;
        }

        void LoadSettings()
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                _currentSettings = JsonUtility.FromJson<PlayerSettings>(json);
            }
            else
            {
                _currentSettings = new PlayerSettings();
                SaveSettings();
            }
        }

        void SaveSettings()
        {
            string json = JsonUtility.ToJson( _currentSettings, true);
            File.WriteAllText(_settingsPath, json);
        }
    }
}
