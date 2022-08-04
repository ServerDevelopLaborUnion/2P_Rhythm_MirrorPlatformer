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

            if (sdJSON == "") 
            { 
                sd = new StageData() { unlockedStage = new List<string>(), };
                for(int i = 1; i < 6; i++)
                    for(int j = 1; j < 5; j++)
                        if(!sd.unlockedStage.Contains(i + "-" + j))
                            sd.unlockedStage.Add(i + "-" + j);
            }
            else sd = JsonUtility.FromJson<StageData>(sdJSON);

            if(udJSON == "") ud = new UserData() { isHost = false, skin = null, };
            else ud = JsonUtility.FromJson<UserData>(udJSON);
        }

        private void SaveStageData()
        {
            string JSON = JsonUtility.ToJson(sd);
            PlayerPrefs.SetString("sdJSON", JSON);
        }

        private void SaveUserData()
        {
            ud.isHost = false;
            string JSON = JsonUtility.ToJson(ud);
            PlayerPrefs.SetString("udJSON", JSON);
        }

        private void OnDestroy()
        {
            SaveStageData();
            SaveUserData();
        }
    }
}
