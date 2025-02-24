using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

namespace intheclouds
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (EnemyManager.instance.GetEnemyCount() == 0 && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SFXPlayer.Instance.PlaySFX(SFXPlayer.Instance.goalReachedSFX, transform.position, 1, 1.3f, 10, false, false);
                EnemyManager.instance.finishedImage.SetActive(true);
            }
        }
    }
}
