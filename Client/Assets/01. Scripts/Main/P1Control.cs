using Newtonsoft.Json;
using Core;
using UnityEngine;

namespace Main
{
    public class P1Control : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer, obstacleLayer;
        [SerializeField] bool isJump = true, isHold = true;
        private Rigidbody2D rb2d = null;
        private SpriteRenderer spR = null;
        private PlayerJump jump = null;
        private PlayerHold hold = null;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            jump = GetComponent<PlayerJump>();
            hold = GetComponent<PlayerHold>();
            spR = GetComponent<SpriteRenderer>();

            spR.sprite = DataManager.Instance.ud.skin;
        }
        
        private void Update()
        {
            Crash();

            if(jump.IsGround())
                hold.IsHolded = false;

            if (Input.GetKeyDown(KeyCode.Space) && isJump)
                jump.DoJump();
            
            if(Input.GetKeyDown(KeyCode.A) && isHold)
                hold.DoHold();

            if(Input.GetKeyUp(KeyCode.A) && isHold)
                hold.StopHold();

            Client.MovePacket mp = new Client.MovePacket(transform.position.x, -transform.position.y);
            string JSON = JsonConvert.SerializeObject(mp);
            Client.Instance.SendMessages("game", "move", JSON);

            #region 
            // if (Input.GetKeyDown(KeyCode.Space) && isJump)
            //     jump.DoJump(() => {
            //         Client.Instance.SendMessages("game", "input", "jump");
            //     });
            
            // if(Input.GetKeyDown(KeyCode.A) && isHold)
            //     hold.DoHold(() => {
            //         Client.Instance.SendMessages("game", "input", "holdS");
            //     });

            // if(Input.GetKeyUp(KeyCode.A) && isHold)
            //     hold.StopHold(() => {
            //         Client.Instance.SendMessages("game", "input", "holdD");
            //     });
            #endregion
        }

        private void Crash()
        {
            RaycastHit2D hitRight = Physics2D.Raycast(transform.localPosition, Vector2.right, transform.localScale.x / 2, groundLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.localPosition, Vector2.up, transform.localScale.x / 2, groundLayer);

            bool isCrash = Physics2D.OverlapBox(transform.localPosition, transform.localScale, obstacleLayer);

            if(hitRight || hitUp || isCrash)
                Client.Instance.SendMessages("game", "dead", "");
        }
    }
}
