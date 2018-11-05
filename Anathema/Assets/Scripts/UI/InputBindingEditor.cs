using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.UI
{
    public class InputBindingEditor : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI title;
        private InputBinding binding;
        public InputBinding Binding
        {
            get
            {
                return binding;        
            }
            set
            {
                binding = value;
                title.text = binding.path;
            }
        }
    }
}