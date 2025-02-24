using System;
using System.Collections.Generic;
using HighlightPlus;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Weapons.Guns;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace intheclouds
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyType
        {
            Blob,
            Runt,
        }

        public EnemyType enemyType;
        public AudioClip damagedSFX;
        public AudioClip diedSFX;
        public GameObject weaponDrop;
        public Slider healthBar;
        public List<MeshRenderer> meshRenderers;

        public HVRDamageHandler _damageHandler { get; protected set; }
        protected HighlightEffect _highlightEffect;
        protected Rigidbody _rb;

        protected virtual void Awake()
        {
            _damageHandler = GetComponent<HVRDamageHandler>();
            _highlightEffect = GetComponent<HighlightEffect>();
            _rb = GetComponent<Rigidbody>();

            if (_damageHandler)
            {
                _damageHandler.damaged += OnDamaged;
                _damageHandler.lifeReachedZero += OnDied;
            }
        }

        protected virtual void Start()
        {
            EnemyManager.instance.AddEnemy(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Projectile"))
            {
                var bullet = other.GetComponent<HVRBullet>();
                if (bullet.deflected)
                {
                    _damageHandler.TakeDamage(5);
                    bullet.gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnDied()
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }

            _rb.detectCollisions = false;

            if (healthBar)
            {
                healthBar.gameObject.SetActive(false);
            }

            SFXPlayer.Instance.PlaySFX(diedSFX, transform.position, 1f, 1.5f);
            EnemyManager.instance.RemoveEnemy(this);
            if (weaponDrop)
            {
                Instantiate(weaponDrop, transform.position, Quaternion.identity);
            }
        }

        protected virtual void OnDamaged()
        {
            SFXPlayer.Instance.PlaySFX(damagedSFX, transform.position, 1f, 1.5f);
            _highlightEffect.HitFX();
            healthBar.value = (float) _damageHandler.Life / _damageHandler.StartingLife;
        }
    }
}