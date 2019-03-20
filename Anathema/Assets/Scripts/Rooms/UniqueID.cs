using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Anathema.Rooms
{
    /// <summary>
    /// <see cref="UniqueID"/> identifies an object by its scene name and own nam.
    /// </summary>
    /// <remarks>
    /// <see cref="UniqueComponent"/> uses ths to find any MonoBehaviour across the loaded scenes.
    /// It is the basis of Anathema's SceneLoading and Saving systems.
    /// </remarks>
    [Serializable]
    public class UniqueID
    {
        [HideInInspector] [SerializeField] private bool useCurrentSceneName = false; //Serializable else HideInInspectorIf won't work.

        [Tooltip("The name of the scene where the referenced object is.")] [HideInInspectorIf(nameof(useCurrentSceneName))]
        [SerializeField] private string sceneName;

        [Tooltip("The name of the object this references.")]
        [SerializeField] private string objectName;

        /// <summary>
        /// Controls hiding of <see cref="sceneName"/> in inspector.
        /// The custom one is the default, but the creating class can set this to true by using the constructor parameter.
        /// Do note that this does NOT automatically set <see cref="sceneName"/> to the current scene name, as UniqueID is not a MonoBehaviour.
        /// </summary>
        public bool UseCurrentSceneName { get { return useCurrentSceneName; } set { useCurrentSceneName = value; } }

        /// <summary>
        /// The name of the scene where the referenced object is.
        /// </summary>
        public string SceneName { get { return sceneName; } set { sceneName = value; } }
        
        /// <summary>
        /// The name of the object this references.
        /// </summary>
        public string ObjectName { get { return objectName; } }

        public UniqueID()
        {

        }
        
        /// <param name="sceneName">Sets <see cref="SceneName"/></param>
        public UniqueID(string sceneName)
        {
            this.sceneName = sceneName;
        }
        
        /// <param name="useCurrentSceneName">Sets <see cref="UseCurrentSceneName"/></param>
        /// <param name="sceneName">Sets <see cref="SceneName"/></param>
        public UniqueID(bool useCurrentSceneName, string sceneName = null)
        {
            this.useCurrentSceneName = useCurrentSceneName;
            this.sceneName = sceneName;
        }
        
        /// <param name="sceneName">Sets <see cref="SceneName"/></param>
        /// <param name="objectName">Sets <see cref="ObjectName"/></param>
        public UniqueID(string sceneName, string objectName)
        {
            this.sceneName = sceneName;
            this.objectName = objectName;
        }
        
        /// <summary>
        /// Converts this <see cref="UniqueID"/> to a string representation.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when object in invalidState: Either <see cref="SceneName"/> or <see cref="ObjectName"/> is null.
        /// </exception>
        public override string ToString()
        {
            if (SceneName != null && ObjectName != null)
            {
                return $"{SceneName}.{ObjectName}";
            }
            else
            {
                throw new InvalidOperationException($"{nameof(ToString)}: Can't convert object in invalid state.");
            }
        }
        
        public override int GetHashCode()
        {
            return ToString().GetHashCode(); 
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is UniqueID)) //If null "is" returns false
            {
                return false;
            }
            else
            { 
                UniqueID uid = (UniqueID) obj; 
                return SceneName == uid.SceneName && ObjectName == uid.ObjectName;
            }   
        }
    }
}