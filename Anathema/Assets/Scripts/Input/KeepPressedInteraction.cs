using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.Input 
{
    public class KeepPressedInteraction : IInputInteraction
    {
        public void Process(ref InputInteractionContext context)
        {
            float value = Mathf.Abs(context.ReadValue<float>());
            if(context.action.phase == InputActionPhase.Waiting && value > 0f)
            {
                context.Started();
            }
            else if (context.action.phase == InputActionPhase.Started && value == 0f)
            {
                context.Performed();
            }
        }

        public void Reset()
        {
        }
    }
}

