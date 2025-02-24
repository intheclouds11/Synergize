using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

namespace intheclouds
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        private WeaponType _weaponType;
        [SerializeField]
        private AudioClip pickupSFX;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SFXPlayer.Instance.PlaySFX(pickupSFX, LocalUserObjects.instance.Camera.transform.position, 1, 1, 10, false, false);
            
                PlayerInventory.instance.AddWeapon(_weaponType);
                Destroy(gameObject);
            }
        }
    }
}