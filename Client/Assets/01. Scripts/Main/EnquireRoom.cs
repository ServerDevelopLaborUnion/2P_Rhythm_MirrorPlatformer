using UnityEngine;

namespace Main
{
    public class EnquireRoom : MonoBehaviour
    {
        public void DoEnquire()
        {
            RoomManager.Instance.RoomUpdate();
        }
    }
}
