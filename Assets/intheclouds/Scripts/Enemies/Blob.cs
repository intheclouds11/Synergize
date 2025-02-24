using HurricaneVR.Framework.Core;
using UnityEngine;

namespace intheclouds
{
    public class Blob : Enemy
    {
        public float boost = 7f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerRB = HVRManager.Instance.PlayerController.playerRigidBody;
                playerRB.linearVelocity = Vector3.zero;
                playerRB.AddForce(0, boost * playerRB.mass, 0, ForceMode.Impulse);
                OnDied();
                Destroy(gameObject);
            }
        }
    }
}