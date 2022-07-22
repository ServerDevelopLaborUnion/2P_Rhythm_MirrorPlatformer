using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core
{
    public class Buttons : MonoBehaviour
    {
        public void Reset()
        {
            if(EventSystem.current.currentSelectedGameObject == null) return;
            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            button.interactable = false;
            button.interactable = true;
        }
    }
}
