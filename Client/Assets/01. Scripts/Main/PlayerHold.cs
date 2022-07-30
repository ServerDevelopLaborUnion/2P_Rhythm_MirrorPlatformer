using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class PlayerHold : MonoBehaviour
    {
        [SerializeField] float duration = 1f;
        private IEnumerator coroutine = null;
        private Rigidbody2D rb2d = null;


        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void DoHold(Action callBack = null)
        {
            coroutine = Hold();
            callBack?.Invoke();
            //rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;
            StartCoroutine(coroutine);
        }

        public void StopHold(Action callBack = null)
        {
            if(coroutine == null) return;
            //rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            StopCoroutine(coroutine);
            rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            callBack?.Invoke();
        }

        private IEnumerator Hold()
        {
            rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;
            float currentTime = 0;

            while(currentTime <= duration)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            rb2d.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }
}