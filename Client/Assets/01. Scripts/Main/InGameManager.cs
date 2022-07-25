using Core;
using UnityEngine;

namespace Main
{
    public class InGameManager : Buttons
    {
        public static InGameManager Instance = null;

        private Canvas canvas = null;
        private GameObject stagePanel = null, loadingPanel = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple InGameManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) Instance = this;

            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            stagePanel = canvas.transform.Find("Panels/StagePanel").gameObject;
            loadingPanel = canvas.transform.Find("Panels/LoadingPanel").gameObject;

            if(DataManager.Instance.ud.isHost)
                loadingPanel.SetActive(false);
            if(!DataManager.Instance.ud.isHost)
                stagePanel.SetActive(false);
        }

        public void StartGame(string stageIndex)
        {
            Reset();
            
            stagePanel.SetActive(false);
            loadingPanel.SetActive(false);
            
            Client.Instance.SendMessages("game", "start", stageIndex);
        }
    }
}
