using System.Security.Cryptography.X509Certificates;
using System;
using UnityEngine;

namespace Main
{
    public class P2Control : MonoBehaviour
    {
        [Flags]
        public enum Events
        {
            Idle = 0,
            Jump = 1,
            HoldS = 1 << 1,
            HoldD = 1 << 2,
        }
        public Events events = Events.Idle;

        public static P2Control Instance = null;

        private PlayerJump jump = null;
        private PlayerHold hold = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;

            jump = GetComponent<PlayerJump>();
            hold = GetComponent<PlayerHold>();
        }

        private void FixedUpdate()
        {
            ResEvent(Events.Jump, () => jump.DoJump() );
            ResEvent(Events.HoldS, () => hold.DoHold() );
            ResEvent(Events.HoldD, () => hold.StopHold() );
        }

        private void ResEvent(Events e, Action action)
        {
            if (events.HasFlag(e))
            {
                action?.Invoke();
                events &= ~e;
            }
        }

        public void ReqEvent(Events e)
        {
            events |= e;
        }
    }
}
