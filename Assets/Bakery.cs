using System;
using UnityEngine;
using DialogueEditor;

namespace DragonspiritGames.PlatformerController
{
    public class Bakery : MonoBehaviour
    {
        [SerializeField] NPCConversation _npcConversation;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //ConversationManager.Instance.StartConversation(_npcConversation);
        }
    }
}
