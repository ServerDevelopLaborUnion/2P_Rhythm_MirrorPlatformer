using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class PoolableMono : MonoBehaviour
    {
        abstract public void Reset();
    }
}
