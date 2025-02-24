using System;
using System.Collections.Generic;
using HurricaneVR.Framework.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace intheclouds
{
    public class HUDController : MonoBehaviour
    {
        public static HUDController instance;
        public Transform hudFollowPoint;
        public float lerpFactor = 100f;
        public float maxSmoothDampDistance = 0.12f;

        [SerializeField]
        private TextMeshProUGUI playerHealthText;
        [SerializeField]
        private TextMeshProUGUI swordText;
        [SerializeField]
        private TextMeshProUGUI pistolText;
        [SerializeField]
        private TextMeshProUGUI smgText;
        [SerializeField]
        private TextMeshProUGUI bowText;
        [SerializeField]
        private TextMeshProUGUI shotgunText;
        [SerializeField]
        private TextMeshProUGUI rpgText;


        [field: SerializeField] public GameObject PointerUI { get; private set; }
        [field: SerializeField] public GameObject LeanWarning { get; private set; }
        [field: SerializeField] public GameObject TeleportCancelReminder { get; private set; }

        private Vector3 _velocity;

        private void Awake()
        {
            instance = this;
            ResetInventoryUI();
        }

        private void Update()
        {
            if (hudFollowPoint)
            {
                var distanceFromFollowPoint = Mathf.Clamp(Vector3.Distance(transform.position, hudFollowPoint.position), 0, maxSmoothDampDistance);
                var lerpTime = Time.deltaTime * lerpFactor * distanceFromFollowPoint;
                transform.position = Vector3.SmoothDamp(transform.position, hudFollowPoint.position, ref _velocity, lerpTime);
            }
        }

        public void UpdateInventoryUI(WeaponType weaponType, int newAmount)
        {
            if (weaponType == WeaponType.Sword)
            {
                swordText.text = $"Sword - {newAmount}";
            }
            else if (weaponType == WeaponType.Pistol)
            {
                pistolText.text = $"Pistol - {newAmount}";
            }
            else if (weaponType == WeaponType.SMG)
            {
                smgText.text = $"SMG - {newAmount}";
            }
            else if (weaponType == WeaponType.Bow)
            {
                bowText.text = $"Bow - {newAmount}";
            }
            else if (weaponType == WeaponType.Shotgun)
            {
                shotgunText.text = $"Shotgun - {newAmount}";
            }
            else if (weaponType == WeaponType.RPG)
            {
                rpgText.text = $"RPG - {newAmount}";
            }
        }

        public void ResetInventoryUI()
        {
            swordText.text = "Sword - ";
            pistolText.text = "Pistol - ";
            smgText.text = "SMG - ";
            bowText.text = "Bow - ";
            shotgunText.text = "Shotgun - ";
            rpgText.text = "RPG - ";
        }

        public void ResetHealthUI()
        {
            playerHealthText.text = "Health: 3/3";
        }

        public void UpdateHealthUI(int remainingHealth)
        {
            playerHealthText.text = $"Health: {remainingHealth}/3 ";
        }
        

        public void ToggleLeanWarning(bool setActive)
        {
            LeanWarning.SetActive(setActive);
            ToggleTeleportCancelReminder(false);
        }

        public void ToggleTeleportCancelReminder(bool setActive)
        {
            TeleportCancelReminder.SetActive(setActive);
        }

        // public void ShowPointerUI(ActionType type, string text)
        // {
        //     // todo: add heal type check. Highest activation priority
        //
        //     if (type == ActionType.Attack && !movementIcon.activeSelf)
        //     {
        //         attackIcon.SetActive(true);
        //         pointerText.text = text;
        //         pointerBackground.SetActive(true);
        //     }
        //     else if (type == ActionType.Selection)
        //     {
        //         headSelectionText.text = text;
        //         headSelectionText.gameObject.SetActive(true);
        //     }
        //     else if (type == ActionType.Movement)
        //     {
        //         attackIcon.SetActive(false);
        //         movementIcon.SetActive(true);
        //         pointerBackground.SetActive(true);
        //         pointerText.text = text;
        //     }
        //
        //     PointerUI.SetActive(true);
        // }

        // public void HidePointerUI(ActionType type)
        // {
        //     //todo: add heal type check. Highest activation priority
        //
        //     if (type == ActionType.Attack)
        //     {
        //         attackIcon.SetActive(false);
        //     }
        //     else if (type == ActionType.Selection)
        //     {
        //         headSelectionText.gameObject.SetActive(false);
        //     }
        //     else if (type == ActionType.Movement)
        //     {
        //         movementIcon.SetActive(false);
        //     }
        //
        //     if (!attackIcon.activeSelf && !movementIcon.activeSelf)
        //     {
        //         pointerBackground.SetActive(false);
        //         pointerText.text = "";
        //
        //         if (!headSelectionText.gameObject.activeSelf)
        //         {
        //             PointerUI.SetActive(false);
        //         }
        //     }
        // }

        // public void ShowEnemyStatusEffectsUI(List<StatusEffect> statusEffects)
        // {
        //     PointerStatusEffects.SetActive(true);
        //     bool updateText = false;
        //
        //     if (currentPointerStatusEffects == null)
        //     {
        //         updateText = true;
        //     }
        //     else
        //     {
        //         foreach (var statusEffect in statusEffects)
        //         {
        //             if (!currentPointerStatusEffects.Contains(statusEffect))
        //             {
        //                 updateText = true;
        //                 break;
        //             }
        //         }
        //     }
        //
        //     if (updateText)
        //     {
        //         pointerStatusEffectsText.text = String.Empty;
        //         foreach (var statusEffect in statusEffects)
        //         {
        //             pointerStatusEffectsText.text += $"{statusEffect.type} for {statusEffect.cooldownTimer} rounds \n";
        //         }
        //     }
        //
        //     currentPointerStatusEffects = statusEffects;
        // }

        // public void HideEnemyStatusEffectsUI()
        // {
        //     PointerStatusEffects.SetActive(false);
        //     currentPointerStatusEffects = null;
        // }
    }
}