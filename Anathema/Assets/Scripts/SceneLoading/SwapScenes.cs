using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Fsm;
using Anathema.Rooms;

namespace Anathema.SceneLoading
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FiniteStateMachine))]
	public class SwapScenes : FsmState
	{
        [SerializeField] private FsmState nextState;
        [SerializeField] private float exitDelay;

        private AsyncOperation newSceneLoadOperation = null;
        public string OldScene { get; set; }
        public UniqueID Destination { get; set; }
        public GameObject Player { get; set; }
        private void OnOldSceneUnloaded(AsyncOperation opertation)
        {
            newSceneLoadOperation.allowSceneActivation = true;
        }
        private void OnNewSceneLoaded(AsyncOperation operation)
        {
            if (exitDelay != 0f)
                Invoke("TransitionStart", exitDelay);
            else
                TransitionStart();
        }
        private void TransitionStart()
        {
            if (Player)
            {
                Player.transform.position = UniqueComponent.Find(Destination)?.transform.position ?? Vector3.zero;
                Player.SetActive(true);
            }   
            fsm.Transition(nextState);
        }
        public override void Enter()
        {
            SceneManager.UnloadSceneAsync(OldScene).completed += OnOldSceneUnloaded;

            newSceneLoadOperation = SceneManager.LoadSceneAsync(Destination.SceneName, LoadSceneMode.Additive);
            newSceneLoadOperation.allowSceneActivation = false;
            newSceneLoadOperation.completed += OnNewSceneLoaded;
        }
        public override void Exit()
        {

        }
	}

}