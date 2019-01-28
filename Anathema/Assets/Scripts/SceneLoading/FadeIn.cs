using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Fsm;

namespace Anathema.SceneLoading
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FiniteStateMachine))]
	public class FadeIn : FsmState
	{
        [SerializeField] private float fadeDuration;
        [SerializeField] private FsmState nextState;
        private Image panel;
        private void FadedIn()
        {
            fsm.Transition(nextState);
        }
        public override void Enter()
        {
            panel = GetComponent<Image>();
            if (!panel) Debug.LogWarning($"{gameObject.name}: Component {GetType()} requires an Image component");

            GetComponent<CanvasRenderer>()?.SetAlpha(0f);
            panel?.CrossFadeAlpha(1f, fadeDuration, false);
            
            Invoke("FadedIn", fadeDuration);
        }
        public override void Exit()
        {
        }
	}

}