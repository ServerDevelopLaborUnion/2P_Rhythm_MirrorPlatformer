using UnityEngine;

namespace Main
{
    public class P1Control : MonoBehaviour
    {
        private PlayerJump jump = null;

        private void Awake()
        {
            jump = GetComponent<PlayerJump>();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jump.DoJump(() => {
                    Client.Instance.SendMessages("jump", "lets do jumping");
                });
        }
    }
}
