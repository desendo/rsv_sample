using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.Common
{
    [RequireComponent(typeof(Button))]
    public class SignalButton<T> : MonoBehaviour where T : new()
    {
        private void Awake()
        {
            var b = GetComponent<Button>();
            b.onClick.AddListener(()=>Di.Instance.Get<SignalBus>().Fire(new T()));
        }
    }
}