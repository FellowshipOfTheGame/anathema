using System;
using UnityEngine;

namespace Anathema
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class HideInInspectorIf : BaseHideInInspectorIf
    {
        public HideInInspectorIf(string conditionName) : base(conditionName, false) {}
    }
}