using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DragonspiritGames.PlatformerController
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] PanelUI _mainMenuPanel;
        [SerializeField] PanelUI _tutorialPanel;
        [SerializeField] PanelUI _accessibilityOptionsPanel;
        [SerializeField] GameObject _optionsExplanationScreen;
        [SerializeField] TextMeshProUGUI _optionsExplanationText;
        [SerializeField] Button _visualPresetText;
        [SerializeField] Button _hearingPresetText;
        [SerializeField] Button _motorSkillsPresetText;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _visualAudio;
        [SerializeField] AudioClip _hearingAudio;
        [SerializeField] AudioClip _motorAudio;

        [SerializeField] AudioClip _descriptiveText;

        List<Button> _tutorialPanelButtons = new List<Button>();

        PlayerInput _inputActions;
        UIController _controller;
        EventSystem _eventSystem;

        private void Awake()
        {
            _controller = GetComponent<UIController>();
            _eventSystem = GetComponent<EventSystem>();
        }

        void Start()
        {
            if (IsFirstTimeUser())
            {
                _tutorialPanel.Activate(true);
                PlayTutorialPanelText();
                _mainMenuPanel.Activate(false);
                //PlayerPrefs.SetInt("FirstTimeUser", 0);
            }
        }

        bool IsFirstTimeUser()
        {
            return !PlayerPrefs.HasKey("FirstTimeUser");
        }


        void PlayTutorialPanelText()
        {
            // This will play the descriptive audio
            _audioSource.PlayOneShot(_descriptiveText);

            //var selectionChange = _controller.input.RetrieveMoveInput(this.gameObject);
            //if (selectionChange > 0)
            //{
                //_eventSystem.SetSelectedGameObject(SelectNextButton());
            //}
            //else
            //{
                //_eventSystem.SetSelectedGameObject(SelectPreviousButton());
            //}
        }

        //GameObject SelectNextButton()
        //{
        //    //if (_eventSystem.currentSelectedGameObject == )
        //}

        public void YesButtonClicked()
        {
            ShowAccessibilityPanel();
        }

        void ShowAccessibilityPanel()
        {
            _mainMenuPanel.Activate(false);
            _tutorialPanel.Activate(false);
            _accessibilityOptionsPanel.Activate(true);
            
        }

        //public void VisualPresetTextSelected()
        //{
        //    _audioSource.PlayOneShot(_visualAudio);
        //    _optionsExplanationScreen.SetActive(true);
        //    _optionsExplanationText.text = "This is a test";
        //}

    }
}
