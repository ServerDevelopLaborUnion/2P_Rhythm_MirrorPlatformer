using System;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class PlayerHold : MonoBehaviour
    {
        private IEnumerator coroutine = null;
        private Rigidbody2D rb2d = null;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void DoHold(Action callBack = null)
        {
            //coroutine = Hold(callBack);
            callBack?.Invoke();
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
            //StartCoroutine(coroutine);
        }

        public void StopHold(Action callBack = null)
        {
            //if(coroutine == null) return;
            rb2d.constraints = RigidbodyConstraints2D.None;
            //StopCoroutine(coroutine);
            callBack?.Invoke();
        }

        private IEnumerator Hold(Action callBack = null)
        {
            callBack?.Invoke();

            while(true)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
                yield return null;
            }
        }
    }
}