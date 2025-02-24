using System;
using HighlightPlus;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Weapons.Guns;
using NaughtyAttributes;
using UnityEngine;

namespace intheclouds
{
    public class ImpactHandler : HVRDamageProvider
    {
        #region Variables

        [Header("Damage Handling")]
        public float damageThreshold = 2f;
        public float deflectThreshold = 2f;
        public float hitCooldown = 0.25f;
        private float hitCooldownTimer;
        public float timeAllowedBetweenEnterAndExitCols = 0.5f;
        private float currentTimeBetweenCols;
        private bool inEnemyCollider;
        private bool inSwipeEnterAngle;

        [Header("SFX Handling")]
        public AudioClip HitEnemyClip;
        public AudioClip DeflectClip;
        public float MinPitch = 0.95f;
        public float MaxPitch = 1;
        public float MaxVolume = 0.5f;
        public AudioClip SwipeClip;
        [ShowIf("showSwipe")]
        public float SwipeThreshold = 3f;
        [ShowIf("showSwipe")]
        public float SwipeCooldownThreshold = 1f;
        [ShowIf("showSwipe")]
        public float MinPitchSwipe = 1f;
        [ShowIf("showSwipe")]
        public float MaxPitchSwipe = 1;
        [ShowIf(nameof(showSwipe))]
        public float MaxVolumeSwipe = 1f;
        [ShowIf(nameof(showSwipe))]
        public float VolumeModifierSwipe = 2f;

        public event Action AppliedDamage;

        private AudioSource _impactAudioSource;
        private AudioSource _swipeAudioSource;
        private Rigidbody _rb;
        private HVRGrabbable _grabbable;
        private HighlightEffect _highlightEffect;
        private Vector3 grabbablePrevPos;
        private Vector3 grabbableCurVel;
        private Vector3 playerPrevPos;
        private Vector3 playerCurVel;

        private bool showSwipe => SwipeClip;

        #endregion


        protected override void Start()
        {
            base.Start();

            _rb = GetComponent<Rigidbody>();
            if (!_rb)
            {
                _rb = GetComponentInParent<Rigidbody>();
            }

            _grabbable = GetComponent<HVRGrabbable>();
            _highlightEffect = GetComponent<HighlightEffect>();
        }

        private void Update()
        {
            HandleSwipeSFX();

            if (!HVRManager.Instance.paused)
            {
                if (hitCooldownTimer > 0 && !inEnemyCollider)
                {
                    hitCooldownTimer -= Time.deltaTime;
                }

                if (currentTimeBetweenCols > 0)
                {
                    currentTimeBetweenCols -= Time.deltaTime;
                }
            }

            grabbableCurVel = (_grabbable.transform.position - grabbablePrevPos) / Time.deltaTime;
            grabbablePrevPos = _grabbable.transform.position;
            playerCurVel = (LocalUserObjects.instance.PlayerController.transform.position - playerPrevPos) / Time.deltaTime;
            playerPrevPos = LocalUserObjects.instance.PlayerController.transform.position;
        }


        private void HandleSwipeSFX()
        {
            if (_grabbable && _grabbable.IsHandGrabbed)
            {
                var relativeVelocity = Mathf.Abs(grabbableCurVel.magnitude - playerCurVel.magnitude);

                if ((!_swipeAudioSource || !_swipeAudioSource.isPlaying) && relativeVelocity > SwipeThreshold)
                {
                    // todo use HVRUtilities.Remap() to make volume scale better
                    _swipeAudioSource = PlayVelocityBasedSFX(relativeVelocity, SwipeClip, MinPitchSwipe, MaxPitchSwipe, MaxVolumeSwipe, 10, VolumeModifierSwipe);
                }
                else if (_swipeAudioSource && relativeVelocity < SwipeCooldownThreshold)
                {
                    StartCoroutine(HVRUtilities.FadeOut(_swipeAudioSource, 0.3f));
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_grabbable.IsHandGrabbed) return;

            var relativeVelocity = Mathf.Abs(grabbableCurVel.magnitude - playerCurVel.magnitude);

            if (relativeVelocity >= damageThreshold)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    if (hitCooldownTimer <= 0)
                    {
                        DamageEnemy(other, relativeVelocity);
                    }

                    if (_highlightEffect)
                    {
                        _highlightEffect.highlighted = true;
                    }
                }
                else if (other.gameObject.CompareTag("SwipeEnterCol"))
                {

                    currentTimeBetweenCols = timeAllowedBetweenEnterAndExitCols;
                }
                else if (other.gameObject.CompareTag("SwipeExitCol"))
                {
                    if (currentTimeBetweenCols > 0)
                    {
                        DamageEnemy(other, relativeVelocity);
                    }
                }
            }

            if (relativeVelocity >= deflectThreshold)
            {
                if (other.gameObject.CompareTag("Projectile"))
                {
                    var bullet = other.GetComponent<HVRBullet>();
                    if (bullet && !bullet.deflected)
                    {
                        SFXPlayer.Instance.PlaySFX(DeflectClip, transform.position, 1f, 1f);
                        var directionToEnemy = (bullet.Gun.transform.position - bullet.transform.position).normalized;
                        bullet.Gun.ChangeBulletDirectionAndSpeed(bullet.gameObject, directionToEnemy, bullet.Gun.BulletSpeed * 1.5f);
                        bullet.deflected = true;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                inEnemyCollider = false;
                
                if (_highlightEffect)
                {
                    _highlightEffect.highlighted = false;
                }
            }
        }

        private void DamageEnemy(Collider hitCollider, float relativeVelocity)
        {
            var damageHandler = hitCollider.GetComponentInParent<HVRDamageHandlerBase>();
            if (damageHandler)
            {
                damageHandler.TakeDamage(Damage);
            }

            PlayVelocityBasedSFX(relativeVelocity, HitEnemyClip, MinPitch, MaxPitch, MaxVolume);
            hitCooldownTimer = hitCooldown;
            inEnemyCollider = true;

            AppliedDamage?.Invoke();
        }

        private AudioSource PlayVelocityBasedSFX(float relativeVelocity, AudioClip clip, float minP, float maxP, float maxVol, float pitchModifier = 0.3f,
            float volumeModifier = 0.25f)
        {
            if (clip)
            {
                var _pitch = Mathf.Clamp(relativeVelocity * pitchModifier, minP, maxP);
                var _volume = Mathf.Clamp(relativeVelocity * volumeModifier, 0, maxVol);
                return SFXPlayer.Instance.PlaySFXAttach(clip, transform, _pitch, _volume, 10);
            }

            return null;
        }
    }
}