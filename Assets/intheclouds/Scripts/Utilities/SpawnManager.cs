using System.Collections.Generic;
using System.Linq;
using Assets.HurricaneVR.Framework.Shared.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace intheclouds
{
    public class SpawnManager : MonoBehaviour
    {
        public static List<PlayerSpawnPoint> PlayerSpawnPoints { get; private set; } = new List<PlayerSpawnPoint>();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            MovePlayerToStartingSpawnPoint();
        }

        private void Start()
        {
            this.ExecuteAfterFixedUpdate(MovePlayerToStartingSpawnPoint);
        }

        public static void MovePlayerToStartingSpawnPoint()
        {
            var spawnPoint = GetStartingPlayerSpawnPoint().transform;
            LocalUserObjects.instance.PlayerController.playerRigidBody.Move(spawnPoint.position, spawnPoint.rotation);
        }

        public static void RegisterUserSpawnPoint(PlayerSpawnPoint playerSpawnPoint)
        {
            PlayerSpawnPoints.Add(playerSpawnPoint);
        }
        
        public static void UnregisterUserSpawnPoint(PlayerSpawnPoint playerSpawnPoint)
        {
            PlayerSpawnPoints.Remove(playerSpawnPoint);
        }

        public static PlayerSpawnPoint GetStartingPlayerSpawnPoint()
        {
            return PlayerSpawnPoints.FirstOrDefault(playerSpawnPoint => playerSpawnPoint.startPosition);
        }
    }
}
