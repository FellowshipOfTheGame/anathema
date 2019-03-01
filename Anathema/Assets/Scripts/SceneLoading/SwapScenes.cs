using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Fsm;
using Anathema.Rooms;
using Anathema.Player;
using Anathema.Saving;

namespace Anathema.SceneLoading
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FiniteStateMachine))]
	public class SwapScenes : FsmState
	{
        [SerializeField] private FsmState nextState;
        [SerializeField] private float exitDelay;

        private AsyncOperation newSceneLoadOperation = null;
        private AsyncOperation playerSceneLoadOperation = null;
        private bool newSceneLoaded = false;
        private bool playerSceneLoaded = false;
        public bool ReloadPlayerScene { get; set; }
        public string OldScene { get; set; }
        public UniqueID Destination { get; set; }
        public string PlayerScene { get; set; }
        public GameObject Player { get; set; }
        public GameData GameData { get; set; }
        private void OnOldSceneUnloaded(AsyncOperation opertation)
        {
            if (playerSceneLoadOperation != null)
                playerSceneLoadOperation.allowSceneActivation = true;

            newSceneLoadOperation.allowSceneActivation = true;
        }
        private void OnNewSceneLoaded(AsyncOperation operation)
        {
            newSceneLoaded = true;
            AttemptTransition();
        }
        private void OnPlayerSceneLoaded(AsyncOperation operation)
        {
            playerSceneLoaded = true;
            AttemptTransition();
        }
        private void AttemptTransition()
        {
            if (newSceneLoaded && (PlayerScene == null || playerSceneLoaded))
                if (exitDelay != 0f)
                    Invoke("TransitionStart", exitDelay);
                else
                    TransitionStart();
        }
        private void TransitionStart()
        {
            if (PlayerScene != null)
            {
                GameObject[] objects = SceneManager.GetSceneByName(PlayerScene).GetRootGameObjects();
                foreach (var obj in objects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        Player = obj;
                        break;
                    }
                }
            }
            if (Player)
            {
                UniqueComponent destinationObject = UniqueComponent.Find(Destination);
                if (destinationObject != null)
                {
                    Door destinationDoor = destinationObject as Door;
                    if (destinationDoor != null)
                    {
                        destinationDoor.IgnoreNextCollision = true;
                    }
                    Player.transform.position = UniqueComponent.Find(Destination)?.transform.position ?? Vector3.zero;
                }
                
                if (GameData != null)
                {
                    PlayerUpgrades playerUpgrades = Player.GetComponent<PlayerUpgrades>();
                    if (playerUpgrades)
                    {
                        playerUpgrades.LoadData(GameData);
                    }
                    else
                    {
                        Debug.Log("SwapScenes: TransitionStart(): Can't find PlayerUpgrades in Player");
                    }
                }
                else
                {
                    Debug.Log("SwapScenes: TransitionStart(): No GameData to load");
                }

                Player.SetActive(true);
                FiniteStateMachine playerFSM = Player.GetComponent<FiniteStateMachine>();
                playerFSM.Transition<Idle>();
            }
            fsm.Transition(nextState);
        }
        public override void Enter()
        {
            SceneManager.UnloadSceneAsync(OldScene).completed += OnOldSceneUnloaded;
            
            if (PlayerScene != null)
            {
                if (ReloadPlayerScene)
                {
                    SceneManager.UnloadSceneAsync(PlayerScene);
                }
                playerSceneLoadOperation = SceneManager.LoadSceneAsync(PlayerScene, LoadSceneMode.Additive);
                playerSceneLoadOperation.allowSceneActivation = false;
                playerSceneLoadOperation.completed += OnPlayerSceneLoaded;
            }

            newSceneLoadOperation = SceneManager.LoadSceneAsync(Destination.SceneName, LoadSceneMode.Additive);
            newSceneLoadOperation.allowSceneActivation = false;
            newSceneLoadOperation.completed += OnNewSceneLoaded;
        }
        public override void Exit()
        {

        }
	}

}