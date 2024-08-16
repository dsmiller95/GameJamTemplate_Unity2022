using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public interface IDestroyLate
    {
        public void DestroyLate();
    }
    [DefaultExecutionOrder(1000)]
    public class LateDestroyHook : MonoBehaviour
    {
        private void OnDestroy()
        {
            foreach (var destroyLate in GetComponents<IDestroyLate>())
            {
                destroyLate.DestroyLate();
            }
        }
    }
}