using System;
using HurricaneVR.Framework.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace HurricaneVR.Framework.Components
{
    public class HVRDamageHandler : HVRDamageHandlerBase
    {
        public int Life = 100;
        public int StartingLife { get; private set; }

        public bool Damageable = true;

        public Rigidbody Rigidbody { get; private set; }

        public HVRDestructible Destructible;
        public GameObject SpawnOnDied;

        public event Action damaged; 
        public event Action lifeReachedZero;

        private void Awake()
        {
            StartingLife = Life;
            Rigidbody = GetComponent<Rigidbody>();
            if (!Destructible)
                Destructible = GetComponent<HVRDestructible>();
            
        }
        
        public override void TakeDamage(int damage)
        {
            if (Damageable)
            {
                Life -= damage;
            }

            if (Life <= 0)
            {
                if (Destructible)
                {
                    Destructible.Destroy();
                }

                if (SpawnOnDied)
                {
                    Destroy(Instantiate(SpawnOnDied, transform.position, Quaternion.identity), 1.3f);
                }
                
                lifeReachedZero?.Invoke();
            }
            else
            {
                damaged?.Invoke();
            }
        }

        public override void HandleDamageProvider(HVRDamageProvider damageProvider, Vector3 hitPoint, Vector3 direction)
        {
            base.HandleDamageProvider(damageProvider, hitPoint, direction);

            if (Rigidbody)
            {
                Rigidbody.AddForceAtPosition(direction.normalized * damageProvider.Force, hitPoint, ForceMode.Impulse);
            }
        }

    }
}