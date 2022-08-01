using Core;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

namespace Main
{
    public class InGameManager : Buttons
    {
        public static InGameManager Instance = null;

        private CinemachineVirtualCamera mainVCam = null;
        private Transform canvas = null;
        private Button themeButton = null;
        private Button returnButton = null;
        public GameObject StagePanel { get; set; } = null;
        public GameObject LoadingPanel { get; set; } = null;
        public GameObject WaitingPanel { get; set; } = null;
        public GameObject ClearPanel { get; set; } = null;
        public Stage currentStage { get; set; }

        private void Awake()
        {
            if (Instance != null) { Debug.Log($"Multiple InGameManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if (Instance == null) Instance = this;

            mainVCam = GameObject.Find("MainVCam").GetComponent<CinemachineVirtualCamera>();
            canvas = GameObject.Find("Canvas").transform;
            StagePanel = canvas.Find("Panels/ThemePanel").gameObject;
            LoadingPanel = canvas.Find("Panels/LoadingPanel").gameObject;
            WaitingPanel = canvas.Find("Panels/WaitingPanel").gameObject;
            ClearPanel = canvas.Find("Panels/ClearPanel").gameObject;
            themeButton = canvas.Find("Bar/ThemeButton").GetComponent<Button>();
            returnButton = ClearPanel.transform.Find("ReturnButton").GetComponent<Button>();
        }
        private void Start()
        {
            themeButton.interactable = false;

            if(DataManager.Instance.ud.isHost)
                LoadingPanel.SetActive(false);

            if (!DataManager.Instance.ud.isHost)
                WaitingPanel.SetActive(false);
        }

        public void StartGame(string stageIndex)
        {
            Reset();

            if(!DataManager.Instance.sd.unlockedStage.Contains(stageIndex)) 
            {
                TextSpawner.Instance.SpawnText("Stage Not Allowed");
                return;
            }

            Client.Instance.SendMessages("game", "start", stageIndex);
        }

        public void Return()
        {
            Reset();

            Client.Instance.SendMessages("game", "return", "");
        }

        public void StageClear()
        {
            ClearPanel.SetActive(true);

            if(!DataManager.Instance.ud.isHost) returnButton.gameObject.SetActive(false);
            else returnButton.gameObject.SetActive(true);

            if(!DataManager.Instance.sd.unlockedStage.Contains(currentStage.NextStage) && DataManager.Instance.ud.isHost)
                DataManager.Instance.sd.unlockedStage.Add(currentStage.NextStage);
            TextSpawner.Instance.SpawnText($"{currentStage.NextStage} Unlocked!");
        }

        public void UnloadStage()
        {
            themeButton.interactable = false;

            mainVCam.m_Lens.OrthographicSize = 7;
            
            AudioManager.Instance.PauseBGM();

            Destroy(currentStage.gameObject);
           
            if(DataManager.Instance.ud.isHost)
                StagePanel.SetActive(true);
            else
                LoadingPanel.SetActive(true);
        }

        public void LoadStage(string index)
        {
            themeButton.interactable = true;
            StagePanel.SetActive(false);
            LoadingPanel.SetActive(false);

            Stage stage = Resources.Load<Stage>($"Stages/Stage{index}");
            currentStage = Instantiate(stage, Vector3.zero, Quaternion.identity);
            currentStage.Init();

            mainVCam.m_Lens.OrthographicSize = currentStage.Ortho;
        }
    }
}
