using UnityEngine;
using Anathema.Player;
using Anathema.Graphics;

namespace Anathema.Rooms
{
    [RequireComponent(typeof(Health))]
    public class BreakableWall : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectsToEnable;
        [SerializeField] private GameObject[] objectsToDisable;
        private Health health;
        private SpriteBurn spriteBurn;
    
        private void Awake()
        {
            spriteBurn = GetComponent<SpriteBurn>();
            health = GetComponent<Health>();

            if (!health) Debug.LogWarning($"{gameObject.name}: BreakableWall requires a Health component.");
            else
            {
                if (spriteBurn)
                {
                    health.OnDeath += Burn;
                    spriteBurn.OnBurnComplete += Die;
                }
                else
                {
                    health.OnDeath += Die;
                }
            }
        }
        private void ToggleObjectsState()
        {
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
        private void Burn()
        {
            health.OnDeath -= Burn;

            ToggleObjectsState();

            spriteBurn.Burn();
        }
        private void Die()
        {
            if (spriteBurn)
            {
                spriteBurn.OnBurnComplete -= Die;
            }
            else
            {
                health.OnDeath -= Die;
            }

            Destroy(this.gameObject);
        }
    }
}