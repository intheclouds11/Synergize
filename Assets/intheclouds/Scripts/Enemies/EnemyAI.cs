using HurricaneVR.Framework.Weapons.Guns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace intheclouds
{
    public class EnemyAI : MonoBehaviour
    {
        public float detectPlayerDistance = 10f;
        public bool forceHorizonLook;
        private Enemy _enemy;
        private bool _playerDetected;
        private HVRGunBase _gunBase;
        public static bool enemyAIActive = true;


        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _gunBase = GetComponent<HVRGunBase>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Initialize();
        }

        private void Initialize()
        {
            _playerDetected = false;
        }

        private void Update()
        {
            if (!enemyAIActive) return;
            
            var distToPlayer = Vector3.Distance(transform.position, LocalUserObjects.instance.PlayerController.transform.position);
            _playerDetected = distToPlayer <= detectPlayerDistance;

            // todo: check line of sight

            if (_enemy._damageHandler.Life > 0 && _playerDetected && LocalUserObjects.instance.playerDamageHandler.Life > 0)
            {
                transform.LookAt(LocalUserObjects.instance.waist.transform);

                if (forceHorizonLook)
                {
                    var oldRot = transform.eulerAngles;
                    oldRot.x = 0;
                    transform.eulerAngles = oldRot;
                }

                if (_enemy.enemyType == Enemy.EnemyType.Runt)
                {
                    _gunBase.EnemyFiring(true);
                }
            }
            else
            {
                if (_gunBase)
                {
                    _gunBase.EnemyFiring(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _playerDetected = true;
            }
        }
    }
}