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
            StartCoroutine(coroutine);
        }

        public void StopHold(Action callBack = null)
        {
            if(coroutine == null || !rb2d.constraints.HasFlag(RigidbodyConstraints2D.FreezePositionY)) return;
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