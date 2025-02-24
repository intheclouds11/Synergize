using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HurricaneVR.Framework.Shared.Utilities;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Shared;
using HurricaneVR.Framework.Weapons.Guns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace intheclouds
{
    public class PlayerInventory : MonoBehaviour
    {
        public static PlayerInventory instance;
        public int sword;
        public int pistolAmount;
        public int smgAmount;
        public int bowAmount;
        public int shotgunAmount;
        public int rpgAmount;

        public List<HVRGunBase> gunBases;

        private LocalUserObjects _localUserObjects;
        [SerializeField]
        private List<WeaponType> weaponHistory = new List<WeaponType>();


        private void Awake()
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            _localUserObjects = LocalUserObjects.instance;
            _localUserObjects.rightHandGrabber.HandGrabbedFromSocket += HandGrabbedFromSocket;

            StartCoroutine(SetupGunBases());
        }

        private IEnumerator SetupGunBases()
        {
            yield return null;
            yield return null;

            foreach (var gunBase in gunBases)
            {
                gunBase.Discarded += () => RemoveWeapon(gunBase.WeaponType, true);
                gunBase.Enable(false);
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            ResetInventory();
        }

        public void AddWeapon(WeaponType type)
        {
            int newAmount;

            if (type == WeaponType.Pistol && pistolAmount <= 2)
            {
                pistolAmount += 1;
                newAmount = pistolAmount;
            }
            else if (type == WeaponType.SMG && smgAmount <= 2)
            {
                smgAmount += 1;
                newAmount = smgAmount;
            }
            else
            {
                return;
            }

            HUDController.instance.UpdateInventoryUI(type, newAmount);

            var rightHandGrabbable = _localUserObjects.rightHandGrabber.GrabbedTarget;

            foreach (var gunBase in gunBases.Where(gunBase => gunBase.WeaponType == type))
            {
                if (gunBase.Grabbable != rightHandGrabbable) // Add and equip
                {
                    if (rightHandGrabbable)
                    {
                        rightHandGrabbable.ForceRelease();
                    }

                    if (!weaponHistory.Contains(type))
                    {
                        weaponHistory.Add(type);
                    }

                    gunBase.Enable(true, true);
                    _localUserObjects.rightHandGrabber.Grab(gunBase.Grabbable, HVRGrabTrigger.Toggle);
                }
                else
                {
                    gunBase.Ammo.OnWeaponAdded();
                }

                break;
            }
        }

        public void RemoveWeapon(WeaponType type, bool discarded = false)
        {
            int newAmount;

            if (type == WeaponType.Pistol && pistolAmount > 0)
            {
                pistolAmount -= 1;
                newAmount = pistolAmount;
            }
            else if (type == WeaponType.SMG && smgAmount > 0)
            {
                smgAmount -= 1;
                newAmount = smgAmount;
            }
            else
            {
                return;
            }

            HUDController.instance.UpdateInventoryUI(type, newAmount);

            if (newAmount == 0)
            {
                // Release weapon, turn weapon socket off
                foreach (var gunBase in gunBases)
                {
                    if (gunBase.WeaponType == type)
                    {
                        weaponHistory.Remove(type);
                        gunBase.Enable(false);
                    }
                }

                // switch to last used weapon where amount > 0
                if (weaponHistory.Count > 0)
                {
                    var lastEquippedType = weaponHistory.Last();
                    Debug.Log($"Last equipped: {lastEquippedType}");

                    this.ExecuteNextUpdate(() => Delay(lastEquippedType));
                }
            }
        }

        private void Delay(WeaponType lastEquippedType)
        {
            foreach (var gunBase in gunBases)
            {
                if (gunBase.WeaponType == lastEquippedType)
                {
                    gunBase.Enable(true);
                    _localUserObjects.rightHandGrabber.Grab(gunBase.Grabbable, HVRGrabTrigger.Toggle);
                }
            }
        }

        /// <summary>
        /// Resort Weapon History when swapping held weapon
        /// </summary>
        /// <param name="grabbable"></param>
        public void HandGrabbedFromSocket(HVRGrabbable grabbable)
        {
            var gunBase = grabbable.GetComponent<HVRGunBase>();
            if (gunBase)
            {
                weaponHistory.Remove(gunBase.WeaponType);
                weaponHistory.Add(gunBase.WeaponType);
            }
        }

        private void ResetInventory()
        {
            HUDController.instance.ResetInventoryUI();
            weaponHistory.Clear();
            sword = 0;
            pistolAmount = 0;
            smgAmount = 0;
            bowAmount = 0;
            shotgunAmount = 0;
            rpgAmount = 0;

            foreach (var gunBase in gunBases)
            {
                gunBase.Enable(false);
            }
        }
    }
}