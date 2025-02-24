using HurricaneVR.Framework.Core;
using UnityEngine;

namespace intheclouds
{
    public class LookAt : MonoBehaviour
    {
        public Transform target;
        public bool followTargetTransformUp;

        private void Start()
        {
            if (!target)
            {
                target = HVRManager.Instance.Camera.transform;
            }
        }

        private void Update()
        {
            if (target)
            {
                if (followTargetTransformUp)
                {
                    transform.rotation = Quaternion.LookRotation(transform.position - target.position, target.up);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(transform.position - target.position);
                }
            }
        }
    }
}