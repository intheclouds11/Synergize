using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace intheclouds
{
    public class UserMenu : MonoBehaviour
    {
        public static UserMenu instance;
        public Button[] Tabs;
        public GameObject[] Pages;
        public GameObject[] controllerHints;
        public Toggle SmoothTurnToggle;
        public Toggle DebugModeToggle;
        public Toggle EnemyAIToggle;
        public AudioClip MenuClip;
        public GameObject MenuIcon;
        public float holdTimeRequired = 1;
        public bool menuIsOpen { get; private set; }
        private GameObject canvasGO;
        private bool menuInputTriggered;
        private float holdTimeLeftSecondaryButton;

        private void Start()
        {
            instance = this;
            canvasGO = transform.GetChild(0).gameObject;
            canvasGO.SetActive(false);

            SmoothTurnToggle.SetIsOnWithoutNotify(LocalUserObjects.instance.PlayerController.RotationType == RotationType.Smooth);
            DebugModeToggle.SetIsOnWithoutNotify(HVRManager.Instance.debugMode);
            EnemyAIToggle.SetIsOnWithoutNotify(EnemyAI.enemyAIActive);
        }

        private void Update()
        {
            CheckMenuButton();

            if (menuIsOpen)
            {
                transform.LookAt(2 * transform.position - LocalUserObjects.instance.Camera.transform.position);
            }
        }

        public void ToggleMenu(bool forceShow = false)
        {
            if (!menuIsOpen || forceShow)
            {
                ShowMenu();
            }
            else
            {
                HideMenu();
            }

            menuIsOpen = !menuIsOpen;
        }

        private void ShowMenu()
        {
            var camTrans = LocalUserObjects.instance.Camera.transform;
            transform.position = camTrans.position + camTrans.forward * 1.2f;
            LocalUserObjects.instance.PlayerInputs.UpdateInputs = false;

            HVRManager.Instance.HandleGamePaused(true);
            canvasGO.SetActive(true);
        }

        private void HideMenu()
        {
            LocalUserObjects.instance.PlayerInputs.UpdateInputs = true;
            HVRManager.Instance.HandleGamePaused(false);
            canvasGO.SetActive(false);
        }

        public void Toggle_SmoothTurn(bool smooth)
        {
            LocalUserObjects.instance.PlayerController.RotationType = smooth ? RotationType.Smooth : RotationType.Snap;
            Startup.SaveIntSetting(Startup.SmoothRotationSettingKey , smooth);
        }

        public void Toggle_DebugMode(bool debug)
        {
            if (DebugModeToggle.isOn != debug) // force update toggle UI in case toggled using keyboard
            {
                DebugModeToggle.SetIsOnWithoutNotify(debug);
            }

            HVRManager.Instance.debugMode = debug;
            Startup.SaveIntSetting(Startup.DebugSettingKey , debug);
        }
        
        public void Toggle_EnemyAIActive(bool active)
        {
            EnemyAI.enemyAIActive = active;
            Startup.SaveIntSetting(Startup.EnemyAIActiveSettingKey, active);
        }

        // Currently only used when selecting tabs in menu. Can be called if want to show player specific page
        public void Button_ChangeTab(int tabIndex)
        {
            for (var i = 0; i < Tabs.Length; i++)
            {
                Tabs[i].interactable = i != tabIndex;
                Pages[i].SetActive(i == tabIndex);
            }
        }

        public void Button_CalibrateHeight()
        {
            LocalUserObjects.instance.HVRCameraRig.Calibrate();
        }

        public void Button_Standing()
        {
            var sitStandSetting = LocalUserObjects.instance.HVRCameraRig.SitStanding;
            if (sitStandSetting == HVRSitStand.Sitting)
            {
                LocalUserObjects.instance.HVRCameraRig.SetSitStandMode(HVRSitStand.PlayerHeight);
            }
        }

        public void Button_Seated()
        {
            var sitStandSetting = LocalUserObjects.instance.HVRCameraRig.SitStanding;
            if (sitStandSetting == HVRSitStand.PlayerHeight)
            {
                LocalUserObjects.instance.HVRCameraRig.SetSitStandMode(HVRSitStand.Sitting);
            }
        }

        public void Button_ControllerHints()
        {
            foreach (var controllerHint in controllerHints)
            {
                controllerHint.SetActive(!controllerHint.activeSelf);
            }
        }

        public void Button_ConfirmReturnToHub()
        {
            // if (GameManager.instance.state == GameState.CombatStart)
            // {
            //     GameManager.instance.EndCombat();
            // }
            // else
            // {
            //     _currentUserObjects.PlayerStats.ResetPlayerStatus();
            // }
            //
            // ToggleMenu();
            // _currentUserObjects.ITCPlayerInputs.ResetUserMenuInputs();
            // SceneLoader.instance.GoToSceneAsync("Celestial Hub");
        }

        public void Button_ResetScene()
        {
            HideMenu();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Button_ResetStats()
        {
            // var playerStatsArray = FindObjectsOfType<PlayerStats>();
            // foreach (var playerStats in playerStatsArray)
            // {
            //     playerStats.CurrentHealth = playerStats.statsSO.maxHealth;
            //     playerStats.CurrentPoise = playerStats.statsSO.maxPoise;
            //     playerStats.CurrentMagicArmor = playerStats.statsSO.maxMagicArmor;
            //     playerStats.CurrentAP = playerStats.statsSO.startingAP;
            //     // if (playerStats.InCombat)
            //     // {
            //     //     playerStats.Turn = true;
            //     // }
            // }

            Debug.Log("RESET STATS");
        }

        private void CheckMenuButton()
        {
            var menuButtonState = LocalUserObjects.instance.PlayerInputs.MenuButtonState;
            if (!menuIsOpen)
            {
                if (menuButtonState.Active && !menuInputTriggered)
                {
                    if (holdTimeLeftSecondaryButton == 0)
                    {
                        MenuIcon.SetActive(true);
                        SFXPlayer.Instance.PlaySFX(MenuClip, LocalUserObjects.instance.Camera.transform.position, 1f, 0.5f, 10, false, false);
                    }

                    if (holdTimeLeftSecondaryButton > holdTimeRequired)
                    {
                        ToggleMenu();
                        MenuIcon.SetActive(false);
                        holdTimeLeftSecondaryButton = 0;
                        menuInputTriggered = true;
                        return;
                    }

                    holdTimeLeftSecondaryButton += Time.deltaTime;
                }
                else if (menuButtonState.JustDeactivated)
                {
                    MenuIcon.SetActive(false);
                    holdTimeLeftSecondaryButton = 0;
                    menuInputTriggered = false;
                }
            }
            else if (menuButtonState.JustActivated)
            {
                ToggleMenu();
                MenuIcon.SetActive(false);
                SFXPlayer.Instance.PlaySFX(SFXPlayer.Instance.clickSFX, LocalUserObjects.instance.Camera.transform.position, 0.8f, 0.5f, 10, false, false);
            }
        }
    }
}