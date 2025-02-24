using System;
using UnityEngine;

namespace HurricaneVR.Framework.Shared
{

    [CreateAssetMenu(menuName = "HurricaneVR/Grab Haptics", fileName = "GrabHaptics")]
    public class HVRGrabHaptics : ScriptableObject
    {
        public void Reset()
        {
            HandHover = new HapticData(.02f, .05f, 50f);

            ForceHover = new HapticData(.02f, .05f, 50);

            HandGrab = new HapticData(.04f, .5f, 50f);
            ForceGrab = new HapticData(.04f, .6f, 60f);

            HandRelease = new HapticData(.025f, .2f, 45f);
        }

        [Header("Hand Grabber")]
        public HapticData HandGrab;
        public HapticData HandRelease;
        public HapticData HandHover;

        [Header("Force Grabber")]
        public HapticData ForceGrab;
        public HapticData ForceHover;
        
        [Header("Inputs")]
        public HapticData GripPress;
        public HapticData GripRelease;
        public HapticData TouchpadPress;
        public HapticData TouchpadRelease;
    }

    [Serializable]
    public class HapticData
    {
        public float Duration;
        public float Amplitude;
        public float Frequency;

        public bool Valid => Duration > .001f && Amplitude > .001f && Frequency > .001f;

        public HapticData()
        {

        }

        public HapticData(float duration, float amplitude, float frequency)
        {
            Duration = duration;
            Amplitude = amplitude;
            Frequency = frequency;
        }
    }


}