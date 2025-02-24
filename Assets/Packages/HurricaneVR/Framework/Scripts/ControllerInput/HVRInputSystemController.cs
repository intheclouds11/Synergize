#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

#if USING_OPENXR
using UnityEngine.XR.OpenXR.Input;
#endif

namespace HurricaneVR.Framework.ControllerInput
{

    public class HVRInputSystemController : HVRController
    {
        public static HVRInputActions InputActions = null;
        private InputDevice _inputDevice;

        public bool IsOpenXR;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Cleanup()
        {
            if (InputActions != null)
            {
                InputActions.Disable();
                InputActions.Dispose();
                InputActions = null;
            }
        }

        public static void Init()
        {
            if (InputActions == null)
            {
                InputActions = new HVRInputActions();
                InputActions.Enable();
            }
        }

        protected override void Start()
        {
            base.Start();
            Init();

            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChanged;
        }

        private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added)
            {
                if (device.usages.Contains(CommonUsages.LeftHand) && Side == HVRHandSide.Left || device.usages.Contains(CommonUsages.RightHand) && Side == HVRHandSide.Right)
                {
                    _inputDevice = device;
                }
            }
        }

        protected override void UpdateInput()
        {
            if (!HVRManager.Instance.isDesktopMode)
            {
                if (Side == HVRHandSide.Left)
                {
                    JoystickAxis = InputActions.LeftHand.Primary2DAxis.ReadValue<Vector2>();

                    SetBool(out JoystickClicked, InputActions.LeftHand.Primary2DAxisClick);
                    SetBool(out TrackPadClicked, InputActions.LeftHand.Secondary2DAxisClick);

                    TrackpadAxis = InputActions.LeftHand.Secondary2DAxis.ReadValue<Vector2>();

                    Grip = InputActions.LeftHand.Grip.ReadValue<float>();
                    GripForce = InputActions.LeftHand.GripForce.ReadValue<float>();
                    Trigger = InputActions.LeftHand.Trigger.ReadValue<float>();
                    IndexTrackpadForce = InputActions.LeftHand.Secondary2DAxisForce.ReadValue<float>();

                    SetBool(out PrimaryButton, InputActions.LeftHand.PrimaryButton);
                    SetBool(out SecondaryButton, InputActions.LeftHand.SecondaryButton);

                    SetBool(out PrimaryTouch, InputActions.LeftHand.PrimaryTouch);
                    SetBool(out SecondaryTouch, InputActions.LeftHand.SecondaryTouch);

                    SetBool(out JoystickTouch, InputActions.LeftHand.Primary2DAxisTouch);
                    SetBool(out TrackPadTouch, InputActions.LeftHand.Secondary2DAxisTouch);

                    SetBool(out TriggerTouch, InputActions.LeftHand.TriggerTouch);

                    SetBool(out MenuButton, InputActions.LeftHand.Menu);

                    SetBool(out GripButton, InputActions.LeftHand.GripPress);
                    SetBool(out TriggerButton, InputActions.LeftHand.TriggerPress);
                }
                else
                {
                    JoystickAxis = InputActions.RightHand.Primary2DAxis.ReadValue<Vector2>();

                    SetBool(out JoystickClicked, InputActions.RightHand.Primary2DAxisClick);
                    SetBool(out TrackPadClicked, InputActions.RightHand.Secondary2DAxisClick);

                    TrackpadAxis = InputActions.RightHand.Secondary2DAxis.ReadValue<Vector2>();

                    Grip = InputActions.RightHand.Grip.ReadValue<float>();
                    GripForce = InputActions.RightHand.GripForce.ReadValue<float>();
                    Trigger = InputActions.RightHand.Trigger.ReadValue<float>();
                    IndexTrackpadForce = InputActions.RightHand.Secondary2DAxisForce.ReadValue<float>();

                    SetBool(out PrimaryButton, InputActions.RightHand.PrimaryButton);
                    SetBool(out SecondaryButton, InputActions.RightHand.SecondaryButton);

                    SetBool(out PrimaryTouch, InputActions.RightHand.PrimaryTouch);
                    SetBool(out SecondaryTouch, InputActions.RightHand.SecondaryTouch);

                    SetBool(out JoystickTouch, InputActions.RightHand.Primary2DAxisTouch);
                    SetBool(out TrackPadTouch, InputActions.RightHand.Secondary2DAxisTouch);

                    SetBool(out TriggerTouch, InputActions.RightHand.TriggerTouch);

                    SetBool(out MenuButton, InputActions.RightHand.Menu);

                    SetBool(out GripButton, InputActions.RightHand.GripPress);
                    SetBool(out TriggerButton, InputActions.RightHand.TriggerPress);
                }
            }
            else // DesktopMode
            {
                if (Side == HVRHandSide.Left)
                {
                    JoystickAxis = InputActions.LeftHandDesktop.Primary2DAxis.ReadValue<Vector2>();

                    SetBool(out JoystickClicked, InputActions.LeftHandDesktop.Primary2DAxisClick);
                    SetBool(out TrackPadClicked, InputActions.LeftHandDesktop.Secondary2DAxisClick);

                    TrackpadAxis = InputActions.LeftHandDesktop.Secondary2DAxis.ReadValue<Vector2>();

                    Grip = InputActions.LeftHandDesktop.Grip.ReadValue<float>();
                    GripForce = InputActions.LeftHandDesktop.GripForce.ReadValue<float>();
                    Trigger = InputActions.LeftHandDesktop.Trigger.ReadValue<float>();
                    IndexTrackpadForce = InputActions.LeftHandDesktop.Secondary2DAxisForce.ReadValue<float>();

                    SetBool(out PrimaryButton, InputActions.LeftHandDesktop.PrimaryButton);
                    SetBool(out SecondaryButton, InputActions.LeftHandDesktop.SecondaryButton);

                    SetBool(out PrimaryTouch, InputActions.LeftHandDesktop.PrimaryTouch);
                    SetBool(out SecondaryTouch, InputActions.LeftHandDesktop.SecondaryTouch);

                    SetBool(out JoystickTouch, InputActions.LeftHandDesktop.Primary2DAxisTouch);
                    SetBool(out TrackPadTouch, InputActions.LeftHandDesktop.Secondary2DAxisTouch);

                    SetBool(out TriggerTouch, InputActions.LeftHandDesktop.TriggerTouch);

                    SetBool(out MenuButton, InputActions.LeftHandDesktop.Menu);

                    SetBool(out GripButton, InputActions.LeftHandDesktop.GripPress);
                    SetBool(out TriggerButton, InputActions.LeftHandDesktop.TriggerPress);
                }
                else
                {
                    JoystickAxis = InputActions.RightHandDesktop.Primary2DAxis.ReadValue<Vector2>();

                    SetBool(out JoystickClicked, InputActions.RightHandDesktop.Primary2DAxisClick);
                    SetBool(out TrackPadClicked, InputActions.RightHandDesktop.Secondary2DAxisClick);

                    TrackpadAxis = InputActions.RightHandDesktop.Secondary2DAxis.ReadValue<Vector2>();

                    Grip = InputActions.RightHandDesktop.Grip.ReadValue<float>();
                    GripForce = InputActions.RightHandDesktop.GripForce.ReadValue<float>();
                    Trigger = InputActions.RightHandDesktop.Trigger.ReadValue<float>();
                    IndexTrackpadForce = InputActions.RightHandDesktop.Secondary2DAxisForce.ReadValue<float>();

                    SetBool(out PrimaryButton, InputActions.RightHandDesktop.PrimaryButton);
                    SetBool(out SecondaryButton, InputActions.RightHandDesktop.SecondaryButton);

                    SetBool(out PrimaryTouch, InputActions.RightHandDesktop.PrimaryTouch);
                    SetBool(out SecondaryTouch, InputActions.RightHandDesktop.SecondaryTouch);

                    SetBool(out JoystickTouch, InputActions.RightHandDesktop.Primary2DAxisTouch);
                    SetBool(out TrackPadTouch, InputActions.RightHandDesktop.Secondary2DAxisTouch);

                    SetBool(out TriggerTouch, InputActions.RightHandDesktop.TriggerTouch);

                    SetBool(out MenuButton, InputActions.RightHandDesktop.Menu);

                    SetBool(out GripButton, InputActions.RightHandDesktop.GripPress);
                    SetBool(out TriggerButton, InputActions.RightHandDesktop.TriggerPress);
                }
            }

        }

        private void SetBool(out bool val, InputAction action)
        {
            val = false;
            if (action.activeControl != null)
            {
                var type = action.activeControl.valueType;
                if (type == typeof(bool))
                {
                    val = action.ReadValue<bool>();
                }
                else if (type == typeof(float))
                {
                    val = action.ReadValue<float>() > .5f;
                }
            }
        }

        public override void Vibrate(float amplitude, float duration = 1, float frequency = 1)
        {
            if (HVRSettings.Instance.DisableHaptics) return;

#if USING_OPENXR

            if (IsOpenXR)
            {
                var action = Side == HVRHandSide.Left ? InputActions.LeftHand.Haptics : InputActions.RightHand.Haptics;

                if (action != null && _inputDevice != null)
                {
                    OpenXRInput.SendHapticImpulse(action, amplitude, frequency, duration, _inputDevice);
                    return;
                }
            }
#endif
            base.Vibrate(amplitude, duration, frequency);

        }
    }
}

#endif