using UnityEngine;

namespace Anathema.Graphics
{
    public class ScalePulse : MonoBehaviour
    {
        [SerializeField] private float pulseSpeed;
        [SerializeField] private float pulseRange;
        private Vector2 baseScale;
        private void Start()
        {
            baseScale = transform.localScale;
        }
        private void Update()
        {
            float offset = Mathf.Sin(Time.realtimeSinceStartup * pulseSpeed) * pulseRange;
            transform.localScale = baseScale + new Vector2(offset, offset);
        }
    }
}