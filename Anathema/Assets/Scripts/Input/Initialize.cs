using UnityEditor;
using UnityEngine.Experimental.Input;

namespace Anathema.Input
{
    [InitializeOnLoad]
    public class Initialize
    {
        static Initialize()
        {
        	InputSystem.RegisterControlProcessor(typeof(DeadzoneFloatProcessor), "DeadzoneFloat");
			InputSystem.RegisterInteraction(typeof(KeepPressedInteraction), "KeepPressed");
			InputSystem.RegisterInteraction(typeof(StickFloatInteraction), "StickFloat");
        }
    }
}