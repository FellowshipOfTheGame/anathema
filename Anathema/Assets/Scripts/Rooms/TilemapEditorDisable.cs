using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Anathema.Rooms
{
    [ExecuteInEditMode]
    public class TilemapEditorDisable : MonoBehaviour
    {
        [SerializeField] private bool disableEditorRendering = true;
        [SerializeField] private bool disableInGameRendering = true;
        private TilemapRenderer[] renderers;
        private void Awake()
        {
            renderers = GetComponentsInChildren<TilemapRenderer>(); 
                
            if (Application.isPlaying)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                if (disableEditorRendering)
                {
                    foreach (var renderer in renderers)
                    {
                        Destroy(renderer);
                    }
                }
            }
        }
        private void OnEnable()
        {
            Selection.selectionChanged += UpdateChilds;
        }
        private void OnDisable()
        {
            Selection.selectionChanged -= UpdateChilds;
        }
        private void UpdateChilds()
        {
            if (!Application.isPlaying)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(Selection.Contains(child.gameObject));
                }
                
            }
        }
    }
}