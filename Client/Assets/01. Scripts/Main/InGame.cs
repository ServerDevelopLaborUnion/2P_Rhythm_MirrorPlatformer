using System.Reflection;
using Core;
using UnityEngine;

namespace Main
{
    public class InGame : MonoBehaviour
    {
        private Canvas canvas = null;
        private GameObject stagePanel = null, loadingPanel = null;

        private void Awake()
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            stagePanel = canvas.transform.Find("Panels/StagePanel").gameObject;
            loadingPanel = canvas.transform.Find("Panels/LoadingPanel").gameObject;

            if(DataManager.Instance.ud.isHost)
                loadingPanel.SetActive(false);
            if(!DataManager.Instance.ud.isHost)
                stagePanel.SetActive(false);
        }
    }
}
