using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

namespace DragonspiritGames.PlatformerController
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] GameObject[] _cutScenes;
        [SerializeField] GameObject _mainCamera;
        [SerializeField] GameObject _stateDrivenCamera;
        [SerializeField] GameObject _cutsceneCamera;
        [SerializeField] Player_Input _playerInput;
        
        float _timer = 0;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            //StartCoroutine(PlayCutscene(_cutScenes[0]));
        }

        public IEnumerator PlayCutscene(GameObject cutscene)
        {
            var cutsceneLength = cutscene.GetComponent<PlayableDirector>().playableAsset.duration;
            cutscene.SetActive(true);
            _cutsceneCamera.SetActive(true);
            _playerInput.enabled = false;
            while (_timer < cutsceneLength)
            {
                _timer += Time.deltaTime;
            }
            yield return new WaitForSeconds(5);
            RestoreControl(cutscene);
        }

        public void RestoreControl(GameObject cutscene)
        {
            _mainCamera.transform.position = _cutsceneCamera.transform.position;
            cutscene.SetActive(false);
            _cutsceneCamera.SetActive(false);
            var stateDrivenCam = _stateDrivenCamera.GetComponent<CinemachineStateDrivenCamera>();
            stateDrivenCam.Follow = GameObject.FindWithTag("Player").transform;
            _playerInput.enabled = true;
        }
    }
}
