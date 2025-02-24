using System.Collections;
using HurricaneVR.Framework.Core;
using UnityEngine;

namespace HurricaneVR.Framework.Weapons.Guns
{
    public class HVRAutomaticGun : HVRGunBase
    {
        public float verticalModifier = -10f;
        
        protected override void Awake()
        {
            base.Awake();
        }
        
        public override void DiscardAbilityPressed()
        {
            base.DiscardAbilityPressed();
            
            HVRManager.Instance.PlayerController.verticalOverride = verticalModifier;
            HVRManager.Instance.PlayerController.stomping = true;
        }
    }
}