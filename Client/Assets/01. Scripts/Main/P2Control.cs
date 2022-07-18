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
            Slide = 1 << 1,
        }
        public Events events = Events.Idle;

        public static P2Control Instance = null;

        private PlayerJump jump = null;

        private void Awake()
        {
            if(Instance == null) Instance = this;

            jump = GetComponent<PlayerJump>();
        }

        private void FixedUpdate()
        {
            GetEvent(Events.Jump, () => jump.DoJump() );
        }

        private void GetEvent(Events e, Action action)
        {
            if (events.HasFlag(e))
            {
                action?.Invoke();
                events &= ~e;
            }
        }

        public void SetEvent(Events e)
        {
            events |= e;
        }
    }
}
