using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Weapons;
using TMPro;
using UnityEngine;

namespace intheclouds
{
    public class ITCAmmo : HVRAmmo
    {
        [SerializeField]
        private TextMeshProUGUI ammoText;
    
        private void Update()
        {
            if (ammoText)
            {
                if (gunEnabled)
                {
                    ammoText.text = $"{CurrentCount} / {MaxCount}";
                }
                else
                {
                    ammoText.text = "";
                }
            }
        }
    }
}