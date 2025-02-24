using System;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace intheclouds
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int startingHealth = 3;
        [SerializeField]
        private HVRDamageHandler _hvrDamageHandler;
        [SerializeField]
        private AudioClip damagedSFX;
        [SerializeField]
        private AudioClip diedSFX;

        private float origSaturation;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            _hvrDamageHandler.damaged += OnDamaged;
            _hvrDamageHandler.lifeReachedZero += OnDied;
        }

        private void Start()
        {
            if (LocalUserObjects.instance.globalVolume)
            {
                LocalUserObjects.instance.globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments);
                origSaturation = colorAdjustments.saturation.value;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _hvrDamageHandler.damaged -= OnDamaged;
            _hvrDamageHandler.lifeReachedZero -= OnDied;
        }

        private void OnDamaged()
        {
            SFXPlayer.Instance.PlaySFX(damagedSFX, LocalUserObjects.instance.Camera.transform);
            LocalUserObjects.instance.globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments);
            colorAdjustments.saturation.value -= 55;

            HUDController.instance.UpdateHealthUI(_hvrDamageHandler.Life);
        }

        private void OnDied()
        {
            SFXPlayer.Instance.PlaySFX(diedSFX, LocalUserObjects.instance.Camera.transform);
            UserMenu.instance.ToggleMenu(true);
            
            LocalUserObjects.instance.globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments);
            colorAdjustments.saturation.value = -100;

            HUDController.instance.UpdateHealthUI(0);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Initialize();
        }

        private void Initialize()
        {
            _hvrDamageHandler.Life = startingHealth;
            HUDController.instance.ResetHealthUI();

            if (LocalUserObjects.instance && LocalUserObjects.instance.globalVolume)
            {
                LocalUserObjects.instance.globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments);
                colorAdjustments.saturation.value = origSaturation;
            }
        }
    }
}