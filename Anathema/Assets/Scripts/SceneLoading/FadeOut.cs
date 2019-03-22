using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Fsm;

namespace Anathema.SceneLoading
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FiniteStateMachine))]
	public class FadeOut : FsmState
	{
        [SerializeField] private float fadeDuration;
        private Image panel;
        private void FadedOut()
        {
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
        public override void Enter()
        {
            panel = GetComponent<Image>();
            if (!panel) Debug.LogWarning($"{gameObject.name}: Component {GetType()} requires an Image component");
            else
            {
                panel.CrossFadeAlpha(0f, fadeDuration, false);
            }

            Invoke(nameof(FadedOut), fadeDuration);
        }
        public override void Exit()
        {
        }
    }

}