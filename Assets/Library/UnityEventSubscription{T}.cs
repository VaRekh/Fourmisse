using System;
using UnityEngine.Events;

namespace Assets.Library
{
    public struct UnityEventSubscription
    {
        public Action<UnityAction> ListenToUnityEvent { get; set; }
        public Action<UnityAction> StopListeningToUnityEvent { get; set; }
    }
    public struct UnityEventSubscription<T>
    {
        public Action<UnityAction<T>> ListenToUnityEvent { get; set; }
        public Action<UnityAction<T>> StopListeningToUnityEvent { get; set; }
    }
}
