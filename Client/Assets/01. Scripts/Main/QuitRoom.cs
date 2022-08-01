using Core;
using UnityEngine;

namespace Main
{
    public class QuitRoom : Buttons
    {
        public void RequestQuit()
        {
            Reset();
            Client.Instance.SendMessages("room", "quitReq", "");
        }
    }
}
