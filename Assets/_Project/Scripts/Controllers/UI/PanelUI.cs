using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DragonspiritGames.PlatformerController
{
    public class PanelUI : MonoBehaviour
    {
        EventSystem _eventSystem;
        public Button[] PanelButtons;

        void Awake()
        {
            _eventSystem = GetComponent<EventSystem>();
            PanelButtons[0].Select();
            Debug.Log($"PanelButton: {PanelButtons[0].name}");
        }

        public void Activate(bool activate = false)
        {
            gameObject.SetActive(activate);
        }
    }
}
