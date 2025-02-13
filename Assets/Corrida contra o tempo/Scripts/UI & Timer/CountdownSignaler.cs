using System;
using UnityEngine;

namespace Corrida
{
    public class CountdownSignaler : MonoBehaviour
    {
        public static event Action CountdownAnimationEnd;

        public void OnAnimationEnd() { CountdownAnimationEnd?.Invoke(); }
    }
}
