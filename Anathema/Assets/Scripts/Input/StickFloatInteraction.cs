using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Interactions;

namespace Anathema.Input
{
    /// <summary>
    /// Starts when stick leaves deadzone, performs while stick moves outside
    /// of deadzone, cancels when stick goes back into deadzone.
    /// </summary>
    public class StickFloatInteraction : IInputInteraction
    {
        public void Process(ref InputInteractionContext context)
        {
            var value = context.ReadValue<float>();
            if (Mathf.Abs(value) > 0f)
            {
                if (!context.isStarted)
                    context.Started();
                else
                    context.PerformedAndStayStarted();
            }
            else if (context.isStarted)
            {
                // Went back to below deadzone.
                context.Cancelled();
            }
        }

        public void Reset()
        {
        }
    }
}
