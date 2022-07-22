using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        private void Awake()
        {
            if(Instance != null) { Debug.Log($"Multiple GameManager Instance is Running, Destroy This"); Destroy(this); }
            if(Instance == null) Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);            
        }
    }
}
