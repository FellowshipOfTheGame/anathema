// GENERATED AUTOMATICALLY FROM 'Assets/Other/Controls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


namespace Anathema
{
    [Serializable]
    public class Controls : InputActionAssetReference
    {
        public Controls()
        {
        }
        public Controls(InputActionAsset asset)
            : base(asset)
        {
        }
        private bool m_Initialized;
        private void Initialize()
        {
            // main
            m_main = asset.GetActionMap("main");
            m_main_Jump = m_main.GetAction("Jump");
            m_main_Crouch = m_main.GetAction("Crouch");
            m_main_Attack = m_main.GetAction("Attack");
            m_main_HorizontalMovement = m_main.GetAction("HorizontalMovement");
            m_Initialized = true;
        }
        private void Uninitialize()
        {
            m_main = null;
            m_main_Jump = null;
            m_main_Crouch = null;
            m_main_Attack = null;
            m_main_HorizontalMovement = null;
            m_Initialized = false;
        }
        public void SetAsset(InputActionAsset newAsset)
        {
            if (newAsset == asset) return;
            if (m_Initialized) Uninitialize();
            asset = newAsset;
        }
        public override void MakePrivateCopyOfActions()
        {
            SetAsset(ScriptableObject.Instantiate(asset));
        }
        // main
        private InputActionMap m_main;
        private InputAction m_main_Jump;
        private InputAction m_main_Crouch;
        private InputAction m_main_Attack;
        private InputAction m_main_HorizontalMovement;
        public struct MainActions
        {
            private Controls m_Wrapper;
            public MainActions(Controls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump { get { return m_Wrapper.m_main_Jump; } }
            public InputAction @Crouch { get { return m_Wrapper.m_main_Crouch; } }
            public InputAction @Attack { get { return m_Wrapper.m_main_Attack; } }
            public InputAction @HorizontalMovement { get { return m_Wrapper.m_main_HorizontalMovement; } }
            public InputActionMap Get() { return m_Wrapper.m_main; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        }
        public MainActions @main
        {
            get
            {
                if (!m_Initialized) Initialize();
                return new MainActions(this);
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get

            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get

            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.GetControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
    }
}
