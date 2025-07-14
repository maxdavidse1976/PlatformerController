using Cinemachine;
using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class AccessibilityManager : MonoBehaviour
    {
        [Header("Layers to show or hide")]
        [SerializeField] GameObject _backgroundsLayer;
        [SerializeField] GameObject _foregroundsLayer;
        
        [Header("Camera settings")]
        [SerializeField] Camera _mainCamera;
        [SerializeField] GameObject _runCamera;
        [SerializeField] GameObject _idleCamera;
        [SerializeField] float _cameraDistance = 3.5f;
        [SerializeField] string _originalBackgroundColor = "#A2F2EB";

        float _idleCameraDistance = 2.5f;
        float _runCameraDistance = 6f;

        public static AccessibilityManager Instance { get; private set; }

        public bool PlayAudioCues { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            // Normally this needs to come from options that we set, but for testing purposes we use an int that we set in Unity.
            PlayerPrefs.SetInt("TurnOffBackgrounds", 0);
            PlayerPrefs.SetInt("TurnOffForegrounds", 0);
            PlayerPrefs.SetInt("TurnBackgroundColorBlack", 0);
            PlayerPrefs.SetInt("TurnOnAudioCues", 0);
            //PlayerPrefs.SetInt("SetCameraDistance", 0);
        }

        void Update()
        {
            ShowBackgrounds();
            ShowForegrounds();
            TurnBackgroundBlack();
            TurnOnAudioCues();
            MakeCameraStatic();
        }
        #region VisibilityOptions

        /// <summary>
        /// Have the option to turn off backgrounds, this checks for the PlayerPrefs to be set to work.
        /// </summary>
        void ShowBackgrounds()
        {
            if (PlayerPrefs.GetInt("TurnOffBackgrounds") == 1)
            {
                _backgroundsLayer.SetActive(false);
            }
            else
            {
                _backgroundsLayer.SetActive(true);
            }
        }

        /// <summary>
        /// Have the option to turn off foregrounds, this checks for the PlayerPrefs to be set to work.
        /// </summary>
        void ShowForegrounds()
        {
            if (PlayerPrefs.GetInt("TurnOffForegrounds") == 1)
            {
                _foregroundsLayer.SetActive(false);
            }
            else 
            {
                _foregroundsLayer.SetActive(true);
            }
        }

        /// <summary>
        ///  Have the option to turn on audio cues, when you need those to play the game. These are also stored in the PlayerPrefs.
        /// </summary>
        void TurnOnAudioCues()
        {
            if (PlayerPrefs.GetInt("TurnOnAudioCues") == 1)
            {
                PlayAudioCues = true;
            }
            else
            {
                PlayAudioCues = false;
            }
        }

        void TurnBackgroundBlack()
        {
            Color color;
            ColorUtility.TryParseHtmlString(_originalBackgroundColor, out color);
            if (PlayerPrefs.GetInt("TurnBackgroundColorBlack") == 1)
            {
                _mainCamera.backgroundColor = Color.black;
            }
            else
            {
                _mainCamera.backgroundColor = color;
            }
        }

        void MakeCameraStatic()
        {
            if (PlayerPrefs.GetInt("SetCameraDistance") == 1)
            {
                _idleCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _cameraDistance;
                _runCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _cameraDistance;
            }
            else
            {
                _idleCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _idleCameraDistance;
                _runCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _runCameraDistance;
            }
        }

        #endregion

        #region HearingOptions
        #endregion

        #region PhysicalOptions
        #endregion
    }
}
