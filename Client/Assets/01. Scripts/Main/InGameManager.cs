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
        public Stage currentStage { get; set; }

        private void Awake()
        {
            if (Instance != null) { Debug.Log($"Multiple InGameManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if (Instance == null) Instance = this;

            mainVCam = GameObject.Find("MainVCam").GetComponent<CinemachineVirtualCamera>();
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            StagePanel = canvas.transform.Find("Panels/StagePanel").gameObject;
            LoadingPanel = canvas.transform.Find("Panels/LoadingPanel").gameObject;
            WaitingPanel = canvas.transform.Find("Panels/WaitingPanel").gameObject;

            if(DataManager.Instance.ud.isHost)
                LoadingPanel.SetActive(false);

            if (!DataManager.Instance.ud.isHost)
                WaitingPanel.SetActive(false);
        }

        public void StartGame(string stageIndex)
        {
            Reset();

            Client.Instance.SendMessages("game", "start", stageIndex);
        }

        public void UnlockStage(string name)
        {
            DataManager.Instance.sd.unlockedStage.Add(name);
        }

        public void LoadStage(string index)
        {
            StagePanel.SetActive(false);
            LoadingPanel.SetActive(false);

            currentStage = Resources.Load<Stage>($"Stages/Stage{index}");
            Instantiate(currentStage, Vector3.zero, Quaternion.identity);
            currentStage.Init();

            mainVCam.m_Lens.OrthographicSize = currentStage.Ortho;
        }
    }
}
