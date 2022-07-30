using System.Security.AccessControl;
using System;
using UnityEngine;

namespace Main
{
    public class PlayerState : MonoBehaviour
    {
        [Flags]
        public enum State
        {
            Idle = 0,
            Jump = 1,
            Hold = 2,
        }

        public State state = State.Idle;

        public void SetState(State state)
        {
            this.state = state;
        }
    }
}
