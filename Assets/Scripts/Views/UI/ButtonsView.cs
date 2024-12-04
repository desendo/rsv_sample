using Game.Services;
using Game.Signals;
using Modules.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ButtonsView : MonoBehaviour, IUIView
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            Di.Instance.Get<IUIViewService>().RegisterView(this);

            _button.onClick.AddListener(() =>
                Di.Instance.Get<SignalBus>()
                    .Fire(new UIViewSignals.OpenPopupRequest<SimplePopupView>()));
        }

        public void Show()
        {
            _button.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _button.gameObject.SetActive(false);
        }
    }
}