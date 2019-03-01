using UnityEngine;

namespace Anathema.Graphics
{
    public class SpriteBurn : MonoBehaviour
    {
        [SerializeField] private float targetBurnTime;
        private new Renderer renderer;
        private float startTime;
        private bool burning = false;
        MaterialPropertyBlock properties;

        private void Start()
        {
            properties = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            if (!renderer)
            {
                Debug.LogWarning("SpriteBurn: Can't find renderer in object " + gameObject.name);
            }
            renderer.GetPropertyBlock(properties);

            properties.SetFloat("_BurnProgress", 0f);
            
            renderer.SetPropertyBlock(properties);
        }

        private void Update()
        {
            if (burning)
            {
                float burnProgress = (Time.realtimeSinceStartup - startTime) / targetBurnTime;
                if (burnProgress >= 1f)
                {
                    burning = false;
                }

                renderer.GetPropertyBlock(properties);

                properties.SetFloat("_BurnProgress", burnProgress);
                
                renderer.SetPropertyBlock(properties);
            }
        }
        public void Burn()
        {
            startTime = Time.realtimeSinceStartup;
            burning = true;
        }

    }
}