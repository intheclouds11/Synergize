using UnityEngine;
using UnityEngine.XR.Management;

namespace intheclouds
{
    public class PermanentObjectInitialization : MonoBehaviour
    {
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PermObjectInitialization()
        {
            if (!XRGeneralSettings.Instance.InitManagerOnStart)
            {
                return;
            }
            var cams = FindObjectsOfType<Camera>();
            foreach (var cam in cams)
            {
                cam.enabled = false;
            }
            var startup = Instantiate(Resources.Load("StartupObjects")) as GameObject;
            DontDestroyOnLoad(startup);
        }
    }
}
