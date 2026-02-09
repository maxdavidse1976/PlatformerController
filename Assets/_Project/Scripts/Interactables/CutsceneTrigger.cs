using UnityEngine;

namespace DragonspiritGames.PlatformerController
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] GameObject _cutSceneToTrigger;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.StartCoroutine(GameManager.Instance.PlayCutscene(_cutSceneToTrigger));
            }
        }
    }
}
