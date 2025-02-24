using UnityEngine;

namespace intheclouds
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public bool startPosition;

        private void OnEnable()
        {
            SpawnManager.RegisterUserSpawnPoint(this);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            SpawnManager.UnregisterUserSpawnPoint(this);
        }
    }
}
