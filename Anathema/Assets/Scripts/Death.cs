using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;
using Anathema.Saving;
using Anathema.Graphics;

namespace Anathema.Player
{
    public class Death : MonoBehaviour
    {
        private Health health;
        private SpriteBurn spriteBurn;
        private SpriteRenderer sRenderer;

        protected virtual void Start()
        {
            health = GetComponent<Health>();
            spriteBurn = GetComponent<SpriteBurn>();
            sRenderer = GetComponent<SpriteRenderer>();

            health.OnDeath += StartBurn;

            spriteBurn.OnBurnComplete += BurnComplete;
        }
        private void StartBurn()
        {
            health.OnDeath -= StartBurn;
            spriteBurn.Burn();

            Component[] components = gameObject.GetComponents<Component>(); 
            
            foreach(var component in components)
            {
                if
                (
                    !( component is Transform
                    || component is SpriteRenderer
                    || component is Death
                    || component is SpriteBurn)
                )
                {
                    Object.Destroy(component);
                }
            }
            
            HandleDeath();
            if (this.gameObject)
            {
                Destroy(this.gameObject);
            }
        }
        private void BurnComplete()
        {
            spriteBurn.OnBurnComplete -= BurnComplete;

            HandleBurnComplete();
        }
        protected virtual void HandleDeath() {}
        protected virtual void HandleBurnComplete() {}
    }
}