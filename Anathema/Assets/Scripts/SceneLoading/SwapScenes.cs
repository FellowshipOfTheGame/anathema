using System.Collections.Generic;
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

        private List<AsyncOperation> sceneLoadOperations = new List<AsyncOperation>();

        private int scenesUnloaded;
        private int scenesLoaded;
        
        public List<string> ScenesToUnload { get; set; } = null;
        public List<string> ScenesToLoad { get; set; } = null;
        public UniqueID Destination { get; set;  } = null;
        public GameData GameData { get; set; } = null;
        
        private void OnSceneUnloaded(AsyncOperation operation)
        {
            scenesUnloaded++;

            if (scenesUnloaded >= ScenesToUnload.Count)
            {
                foreach (var loadOp in sceneLoadOperations)
                {
                    loadOp.allowSceneActivation = true;
                }
            }
        }
        private void OnSceneLoaded(AsyncOperation operation)
        {
            scenesLoaded++;

            if (scenesLoaded >= ScenesToLoad.Count)
            {
                if (exitDelay >= 0f)
                    Invoke(nameof(TransitionStart), exitDelay);
                else
                    TransitionStart();
            }
        }
        private void TransitionStart()
        {
            SceneLoader.OnSceneLoaded?.Invoke(Destination, GameData);
            SceneLoader.OnLateSceneLoaded?.Invoke(Destination, GameData);
            fsm.Transition(nextState);
        }
        public override void Enter()
        {
            foreach (var scene in ScenesToUnload)
            {
                SceneManager.UnloadSceneAsync(scene).completed += OnSceneUnloaded;
            }

            foreach (var scene in ScenesToLoad)
            {
                var loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                
                if (ScenesToUnload.Count > 0)
                    loadOp.allowSceneActivation = false;
                loadOp.completed += OnSceneLoaded;
                
                sceneLoadOperations.Add(loadOp);
            }
        }
        public override void Exit()
        {

        }
	}

}