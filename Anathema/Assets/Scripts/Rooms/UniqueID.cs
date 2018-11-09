using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Rooms
{
    [System.Serializable]
    public class UniqueID
    {
        [HideInInspector] [SerializeField] private bool useCurrentSceneName = false;
        [HideInInspectorIf("useCurrentSceneName")] [SerializeField] private string sceneName;
        [SerializeField] private string objectName;
        public bool UseCurrentSceneName { get { return useCurrentSceneName; } set { useCurrentSceneName = value; } }
        public string SceneName { get { return sceneName; } set { sceneName = value; } }
        public string ObjectName { get { return objectName; } }

        public UniqueID(bool useCurrentSceneName = false)
        {
            this.useCurrentSceneName = useCurrentSceneName;
        }
        public override string ToString()
        {
            return $"{SceneName}.{ObjectName}";
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode(); 
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
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