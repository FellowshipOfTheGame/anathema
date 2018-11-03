using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Rooms
{
    public class UniqueComponent : MonoBehaviour
    {
        private static Dictionary<UniqueID, UniqueComponent> uniques = new Dictionary<UniqueID, UniqueComponent>();
        [SerializeField] private UniqueID uniqueID = new UniqueID(true);
        public UniqueID UniqueID { get { return uniqueID; } }
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