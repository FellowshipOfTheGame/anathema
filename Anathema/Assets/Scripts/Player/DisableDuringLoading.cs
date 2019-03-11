using Anathema.Fsm;
using Anathema.Rooms;
using Anathema.Saving;
using UnityEngine;
using Anathema.SceneLoading;

namespace Anathema.Player
{
    public class DisableDuringLoading : MonoBehaviour
    {
        private FiniteStateMachine fsm;
        private void Awake()
        {
            fsm = GetComponent<FiniteStateMachine>();
            
            SceneLoader.OnSceneAboutToUnload += SceneUnloadHandler;
            SceneLoader.OnLateSceneLoaded += SceneLoadHandler;
        }

        private void SceneUnloadHandler(string sceneName)
        {
            gameObject.SetActive(false);
        }
        private void SceneLoadHandler(UniqueID destination, GameData gameData)
        {
            transform.position = UniqueComponent.Find(destination)?.transform.position ?? Vector3.zero;
            gameObject.SetActive(true);
            fsm.Transition<Idle>();
        }

        private void OnDestroy()
        {
            SceneLoader.OnSceneAboutToUnload += SceneUnloadHandler;
            SceneLoader.OnLateSceneLoaded += SceneLoadHandler;
        }
    }
}