using UnityEngine;

namespace Anathema.Rooms
{
    public class Parallax : MonoBehaviour
    {
        private void FixedUpdate()
        {
            Camera main = Camera.main;
            if (main != null)
                transform.position = (Vector2) main.transform.position;
        }
    }
}