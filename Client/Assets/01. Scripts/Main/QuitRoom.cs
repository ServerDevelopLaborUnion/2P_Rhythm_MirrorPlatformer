using Core;
using UnityEngine;

namespace Main
{
    public class QuitRoom : MonoBehaviour
    {
        public void RequestQuit()
        {
            Client.Instance.SendMessages("room", "quitReq", "");
        }
    }
}
