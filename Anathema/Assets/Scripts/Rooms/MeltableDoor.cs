using Anathema.Graphics;
using UnityEngine;

namespace Anathema.Rooms
{
    public class MeltableDoor : MonoBehaviour
    {
        private SpriteBurn spriteBurn;
        private void Start()
        {
            spriteBurn = GetComponent<SpriteBurn>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Fire fire = other.GetComponent<Fire>();
            if (fire)
            {
                spriteBurn.Burn();
            }
        }
    }
}