using UnityEngine;

namespace HurricaneVR.Framework.Components
{
    public class HVRDamageProvider : MonoBehaviour
    {
        public WeaponType WeaponType;
        public int Damage = 25;
        public float Force;

        [Tooltip("Player transform for ai frameworks like emerald ai")]
        public Transform Player;

        protected virtual void Start()
        {
        
        }

    }
    
    public enum WeaponType
    {
        Sword,
        Pistol,
        SMG,
        Bow,
        Shotgun,
        RPG
    };
}
