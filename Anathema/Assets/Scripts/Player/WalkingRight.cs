using UnityEngine;

namespace Anathema.Player
{
	public class WalkingRight : Walking
    {		
        protected override void StartMovement()
        {
            sRenderer.flipX = false;
            rBody.velocity = moveDirection * baseSpeed;
        }
	}
}