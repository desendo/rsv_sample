using Game.Signals;
using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI
{
    public class SaveLoadButtonsView : MonoBehaviour
    {
        [SerializeField] private Button _save;
        [SerializeField] private Button _load;
        [SerializeField] private Button _reset;

        private void Awake()
        {
            var signalBus = Di.Instance.Get<ISignalBus>();
            _save.onClick.AddListener(() => signalBus.Fire(new UIViewSignals.QuickSaveGameRequest()));
            _load.onClick.AddListener(() => signalBus.Fire(new UIViewSignals.QuickLoadGameRequest()));
            _reset.onClick.AddListener(() => signalBus.Fire(new UIViewSignals.RestartGameRequest()));
        }
    }
}