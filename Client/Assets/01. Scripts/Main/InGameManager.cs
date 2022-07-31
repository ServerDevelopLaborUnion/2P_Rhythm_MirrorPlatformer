using Core;
using UnityEngine;
using Cinemachine;

namespace Main
{
    public class InGameManager : Buttons
    {
        public static InGameManager Instance = null;

        private CinemachineVirtualCamera mainVCam = null;
        private Canvas canvas = null;
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
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            StagePanel = canvas.transform.Find("Panels/ThemePanel").gameObject;
            LoadingPanel = canvas.transform.Find("Panels/LoadingPanel").gameObject;
            WaitingPanel = canvas.transform.Find("Panels/WaitingPanel").gameObject;
            ClearPanel = canvas.transform.Find("Panels/ClearPanel").gameObject;

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

        public void BackToMenu()
        {
            Reset();

            Client.Instance.SendMessages("game", "return", "");
        }

        public void StageClear()
        {
            ClearPanel.SetActive(true);

            UnloadStage();

            if(!DataManager.Instance.sd.unlockedStage.Contains(currentStage.NextStage))
                DataManager.Instance.sd.unlockedStage.Add(currentStage.NextStage);
            TextSpawner.Instance.SpawnText($"{currentStage.NextStage} Unlocked!");
        }

        public void UnloadStage()
        {
            AudioManager.Instance.PauseBGM();

            Destroy(currentStage.gameObject);
           
            if(DataManager.Instance.ud.isHost)
                StagePanel.SetActive(true);
            else
                LoadingPanel.SetActive(true);
        }

        public void LoadStage(string index)
        {
            StagePanel.SetActive(false);
            LoadingPanel.SetActive(false);

            Stage stage = Resources.Load<Stage>($"Stages/Stage{index}");
            currentStage = Instantiate(stage, Vector3.zero, Quaternion.identity);
            currentStage.Init();

            mainVCam.m_Lens.OrthographicSize = currentStage.Ortho;
        }
    }
}
