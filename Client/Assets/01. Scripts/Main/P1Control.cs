using UnityEngine;

namespace Main
{
    public class P1Control : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer, obstacleLayer;
        [SerializeField] bool isJump = true, isHold = true;
        private PlayerJump jump = null;
        private PlayerHold hold = null;

        private void Awake()
        {
            jump = GetComponent<PlayerJump>();
            hold = GetComponent<PlayerHold>();
        }
        
        private void Update()
        {
            Crash();

            if (Input.GetKeyDown(KeyCode.Space) && isJump)
                jump.DoJump(() => {
                    Client.Instance.SendMessages("game", "input", "jump");
                });
            
            if(Input.GetKeyDown(KeyCode.A) && isHold)
                hold.DoHold(() => {
                    Client.Instance.SendMessages("game", "input", "holdS");
                });

            if(Input.GetKeyUp(KeyCode.A) && isHold)
                hold.StopHold(() => {
                    Client.Instance.SendMessages("game", "input", "holdD");
                });
        }

        private void Crash()
        {
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x / 2 + 0.05f, groundLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, transform.localScale.x / 2 + 0.05f, groundLayer);

            bool isCrash = Physics2D.OverlapCircle(transform.position, transform.localScale.x / 2, obstacleLayer);
            
            if(hitRight || hitUp || isCrash)
                Client.Instance.SendMessages("game", "dead", "");
        }
    }
}
