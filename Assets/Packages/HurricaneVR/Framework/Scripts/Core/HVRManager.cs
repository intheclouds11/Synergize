using System.Collections.Generic;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using UnityEngine;

namespace HurricaneVR.Framework.Core
{
    public class HVRManager : MonoBehaviour
    {
        public static HVRManager Instance { get; private set; }

        public HVRGrabberManager GrabberManager;
        public HVRPlayerController PlayerController;
        public HVRHandGrabber LeftHandGrabber;
        public HVRHandGrabber RightHandGrabber;
        public HVRForceGrabber LeftForceGrabber;
        public HVRForceGrabber RightForceGrabber;
        public Transform Camera;
        public HVRScreenFade ScreenFader { get; private set; }

        [Header("Debug")]
        public bool isDesktopMode;
        public bool debugMode;
        public bool paused;
        private Vector3 velocityBeforePause;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                if (!GrabberManager)
                {
                    GrabberManager = gameObject.AddComponent<HVRGrabberManager>();
                }
            }
            else
            {
                Destroy(this);
            }

            var finder = FindObjectOfType<HVRGlobalFadeFinder>();
            if (finder)
            {
                ScreenFader = finder.gameObject.GetComponent<HVRScreenFade>();
            }

            if (!PlayerController)
            {
                PlayerController = FindObjectOfType<HVRPlayerController>();
            }
        }

        public void RegisterGrabber(HVRGrabberBase grabber)
        {
            if (GrabberManager)
            {
                GrabberManager.RegisterGrabber(grabber);
            }
        }

        public void UnregisterGrabber(HVRGrabberBase grabber)
        {
            if (GrabberManager)
            {
                GrabberManager.UnregisterGrabber(grabber);
            }
        }


        public void IgnorePlayerCollision(IEnumerable<Collider> colliders)
        {
            if (PlayerController)
                PlayerController.IgnoreCollision(colliders);
        }

        public void ScreenFade(float alpha, float speed)
        {
            if (ScreenFader)
                ScreenFader.Fade(alpha, speed);
        }

        public void HandleGamePaused(bool pause)
        {
            paused = pause;
            if (paused)
            {
                velocityBeforePause = PlayerController.playerRigidBody.linearVelocity;
                PlayerController.playerRigidBody.linearVelocity = Vector3.zero;
                PlayerController.playerRigidBody.isKinematic = true;

            }
            else
            {
                PlayerController.playerRigidBody.isKinematic = false;
                PlayerController.playerRigidBody.linearVelocity = velocityBeforePause;
            }
            LeftHandGrabber.AllowHovering = !pause;
            RightHandGrabber.AllowHovering = !pause;
            LeftForceGrabber.AllowHovering = !pause;
            RightForceGrabber.AllowHovering = !pause;
        }
    }
}