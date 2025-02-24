using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HurricaneVR.Framework.Shared.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace intheclouds
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;

        [SerializeField]
        private TextMeshProUGUI enemiesRemainingText;
        public GameObject finishedImage;
        [SerializeField]
        private List<Enemy> _enemies = new List<Enemy>();

        private void Awake()
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _enemies.Clear();
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            enemiesRemainingText.text = $"Enemies Remaining: {_enemies.Count}";
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            if (_enemies.Any())
            {
                enemiesRemainingText.text = $"Enemies Remaining: {_enemies.Count}";
            }
            else
            {
                enemiesRemainingText.text = $"REACH THE GOAL!";
            }
        }

        public int GetEnemyCount()
        {
            return _enemies.Count;
        }
    }
}