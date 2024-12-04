using Game.Services;
using Game.Signals;
using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject _pausedScreen;
        [SerializeField] private Button _pauseButton;

        private void Awake()
        {
            Di.Instance.Get<IUpdateProvider>().Paused.Subscribe(HandleIsPaused);
            _pauseButton.onClick.AddListener(() => Di.Instance.Get<ISignalBus>().Fire(new UIViewSignals.TogglePause()));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Di.Instance.Get<ISignalBus>().Fire(new UIViewSignals.TogglePause());
        }

        private void HandleIsPaused(bool isPaused)
        {
            _pausedScreen.SetActive(isPaused);
        }
    }
}