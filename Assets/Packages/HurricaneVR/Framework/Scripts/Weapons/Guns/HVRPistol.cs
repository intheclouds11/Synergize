using System.Collections;
using HurricaneVR.Framework.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace HurricaneVR.Framework.Weapons.Guns
{
    public class HVRPistol : HVRGunBase
    {
        public float verticalModifier = 10f;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public override void DiscardAbilityPressed()
        {
            base.DiscardAbilityPressed();
            
            HVRManager.Instance.PlayerController.verticalOverride = verticalModifier;
        }
    }
}