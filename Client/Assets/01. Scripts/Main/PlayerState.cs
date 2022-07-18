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
        }

        public State state = State.Idle;

        public void SetJump()
        {
            state = State.Jump;
        }

        public void SetIdle()
        {
            state = State.Idle;
        }
    }
}
