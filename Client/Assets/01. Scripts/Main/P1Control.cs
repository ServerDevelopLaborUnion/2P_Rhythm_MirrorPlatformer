using UnityEngine;

namespace Main
{
    public class P1Control : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer, obstacleLayer;
        private PlayerJump jump = null;

        private void Awake()
        {
            jump = GetComponent<PlayerJump>();
        }

        private void Update()
        {
            Crash();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jump.DoJump(() => {
                    Client.Instance.SendMessages("game", "input", "jump");
                });
        }

        private void Crash()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x / 2 + 0.05f, groundLayer);
            bool isCrash = Physics2D.OverlapCircle(transform.position, transform.localScale.x / 2, obstacleLayer);
            if(hit || isCrash)
                Client.Instance.SendMessages("game", "dead", "");
        }
    }
}
