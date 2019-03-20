using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Rooms
{
    /// <summary>
    /// An UniqueComponent is identifiable across scenes by it's UniqueID.
    /// It also can be searched for across loaded scenes quickly.
    /// </summary>
    public class UniqueComponent : MonoBehaviour
    {
        /// <summary>
        /// Associates all loaded UniqueComponents with their IDs.
        /// </summary>
        private static Dictionary<UniqueID, UniqueComponent> uniques = new Dictionary<UniqueID, UniqueComponent>();
        
        [SerializeField] private UniqueID uniqueID = new UniqueID(true);
        
        /// <summary>
        /// The UniqueID of this UniqueComponent.
        /// </summary>
        public UniqueID UniqueID { get { return uniqueID; } }
        
        /// <summary>
        /// Finds an UniqueComponent across loaded scenes by it's UniqueID.
        /// </summary>
        /// <param name="uid">UniqueID of the component</param>
        /// <returns>Null on failure, the component on success</returns>
        public static UniqueComponent Find(UniqueID uid)
        {
            UniqueComponent component = null;
            uniques.TryGetValue(uid, out component);
            return component;
        }
        
        protected virtual void Awake()
        {
            uniqueID.SceneName = gameObject.scene.name;
            uniques.Add(uniqueID, this);
        }
        
        protected virtual void OnDestroy()
        {
            uniques.Remove(uniqueID);
        }
    }
}