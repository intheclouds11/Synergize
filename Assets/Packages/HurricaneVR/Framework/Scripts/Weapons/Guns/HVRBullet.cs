using HurricaneVR.Framework.Components;
using UnityEngine;
using UnityEngine.Events;

namespace HurricaneVR.Framework.Weapons.Guns
{
    public class HVRBullet : MonoBehaviour
    {
        public HVRGunBase Gun { get; set; }
        public bool deflected;
    }
}