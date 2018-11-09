using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class HideInInspectorIf : PropertyAttribute
    {
        public string conditionName;
        public HideInInspectorIf(string conditionName)
        {
			this.conditionName = conditionName;
        }
    }
}