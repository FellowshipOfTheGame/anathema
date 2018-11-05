using UnityEngine;

namespace Anathema.Player
{
	public class WalkingLeft : Walking
    {		
        protected override void StartMovement()
        {
            sRenderer.flipX = true;
            rBody.velocity = Quaternion.Euler(0, 0, 180f) * moveDirection * baseSpeed;
        }
	}
}