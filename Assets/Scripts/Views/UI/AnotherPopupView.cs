using Game.Services;
using Game.Signals;
using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AnotherPopupView : MonoBehaviour, IUIView
    {
        [SerializeField] private Button _buttonClose;
        [SerializeField] private Button _buttonClose2;
        [SerializeField] private GameObject _panel;

        private void Awake()
        {
            Di.Instance.Get<IUIViewService>().RegisterView(this);

            _buttonClose.onClick.AddListener(() =>
                Di.Instance.Get<ISignalBus>()
                    .Fire(new UIViewSignals.ClosePopupRequest<AnotherPopupView>()));
            _buttonClose2.onClick.AddListener(() =>
                Di.Instance.Get<ISignalBus>()
                    .Fire(new UIViewSignals.ClosePopupRequest<AnotherPopupView>()));
        }

        public void Show()
        {
            _panel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }
    }
}