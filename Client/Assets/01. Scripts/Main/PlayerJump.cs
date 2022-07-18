using UnityEngine;
using System;

namespace Main
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] Vector2 dir = Vector2.zero;
        [SerializeField] LayerMask groundLayer = 128;
        private Rigidbody2D rb2d = null;
        private Collider2D col2d = null;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            col2d = GetComponent<Collider2D>();
        }

        public void DoJump(Action callBack = null)
        {
            if(!IsGround()) return;
            
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(dir, ForceMode2D.Impulse);
            
            callBack?.Invoke();
        }

        private bool IsGround()
        {
            return Physics2D.OverlapBox(col2d.bounds.center, col2d.bounds.size, 0, groundLayer);
        }
    }
}
