using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance = null;

        public StageData sd = null;
        public UserData ud = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple DataManager Instance is Running, Destroy This"); Destroy(gameObject); }
            if(Instance == null) { Instance = this; DontDestroyOnLoad(transform.root.gameObject); }

            string sdJSON = PlayerPrefs.GetString("sdJSON", "");
            string udJSON = PlayerPrefs.GetString("udJSON", "");

            if (sdJSON == "") { sd = new StageData() { unlockedStage = new List<string>(), }; sd.unlockedStage.Add("1-1"); }
            else sd = JsonConvert.DeserializeObject<StageData>(sdJSON);

            if(udJSON == "") ud = new UserData() { isHost = false, };
            else ud = JsonConvert.DeserializeObject<UserData>(udJSON);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                for(int i = 1; i < 5; i++)
                    for(int j = 1; j < 5; j++)
                        if(!sd.unlockedStage.Contains(i + "-" + j))
                            sd.unlockedStage.Add(i + "-" + j);
            }
        }

        private void SaveStageData()
        {
            string JSON = JsonConvert.SerializeObject(sd);
            PlayerPrefs.SetString("sdJSON", JSON);
        }

        private void SaveUserData()
        {
            ud.isHost = false;
            string JSON = JsonConvert.SerializeObject(ud);
            PlayerPrefs.SetString("udJSON", JSON);
        }

        private void OnDestroy()
        {
            SaveStageData();
            SaveUserData();
        }
    }
}
