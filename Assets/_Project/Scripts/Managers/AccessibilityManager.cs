using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class AccessibilityManager : MonoBehaviour
    {
        [Header("Layers to show or hide")]
        [SerializeField] GameObject _backgroundsLayer;
        [SerializeField] GameObject _foregroundsLayer;

        void Awake()
        {
            PlayerPrefs.SetInt("TurnOffBackgrounds", 0);
            PlayerPrefs.SetInt("TurnOffForegrounds", 0);
        }

        void Update()
        {
            ShowBackgrounds();
            ShowForegrounds();
        }
        #region VisibilityOptions
        // Normally this needs to come from options that we set, but for testing purposes we use a boolean that we set in Unity.

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
            if (PlayerPrefs.GetFloat("TurnOffForegrounds") == 1)
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

        }
        #endregion

        #region HearingOptions
        #endregion

        #region PhysicalOptions
        #endregion
    }
}
