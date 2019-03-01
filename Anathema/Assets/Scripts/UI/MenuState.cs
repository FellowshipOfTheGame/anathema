using UnityEngine;
using Anathema.Fsm;

namespace Anathema.UI
{
    public class MenuState : FsmState
    {
        [SerializeField] protected GameObject menu;
        [SerializeField] protected float ClickDelay = 0.1f;
        public override void Enter()
        {
            menu.SetActive(true);
        }
        public override void Exit()
        {
            menu.SetActive(false);
        }
    }
}