using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public abstract class UIInputController : ScriptableObject
    {
        public abstract float RetrieveMoveInput(GameObject gameObject);
    }
}
