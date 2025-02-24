using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace intheclouds
{
    public class LocalUserObjects : MonoBehaviour
    {
        public static LocalUserObjects instance;
        public HVRDamageHandler playerDamageHandler;
        public HVRPlayerController PlayerController;
        public HVRTeleporter Teleporter;
        public HVRPlayerInputs PlayerInputs;
        public HVRCameraRig HVRCameraRig;
        public Camera Camera;
        public Transform leftController;
        public HVRHandGrabber leftHandGrabber;
        public Transform rightController;
        public HVRHandGrabber rightHandGrabber;
        public GameObject waist;
        public Volume globalVolume;


        private void Awake()
        {
            instance = this;

            foreach (var volume in FindObjectsOfType<Volume>())
            {
                if (volume.isGlobal)
                {
                    globalVolume = volume;
                    break;
                }
            }

        }
    }
}
