using UnityEngine;

namespace Anathema.Rooms
{
    public class Parallax : MonoBehaviour
    {
        private void Start()
        {
        }
        private void FixedUpdate()
        {
            transform.position = (Vector2) Camera.main.transform.position;

            // (currentCameraPosition - lastCameraPosition);
			//lastCameraPosition = currentCameraPosition;
        }
    }
}