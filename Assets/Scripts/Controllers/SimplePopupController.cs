using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Controllers
{
    public class SimplePopupController
    {
        private readonly IUIViewService _viewService;

        public SimplePopupController(ISignalBus signalBus, IUIViewService viewService)
        {
            _viewService = viewService;
            signalBus.Subscribe<UIViewSignals.OpenPopupRequest<SimplePopupView>>(ProcessOpenSimplePopupViewRequest);
            signalBus.Subscribe<UIViewSignals.ClosePopupRequest<SimplePopupView>>(ProcessCloseSimplePopupViewRequest);
        }

        private void ProcessOpenSimplePopupViewRequest(UIViewSignals.OpenPopupRequest<SimplePopupView> obj)
        {
            _viewService.Hide<ButtonsView>();
            //model
            _viewService.Show<SimplePopupView, SimplePopupData>(new SimplePopupData
            {
                ModelId = "m"
            });
        }

        private void ProcessCloseSimplePopupViewRequest(UIViewSignals.ClosePopupRequest<SimplePopupView> obj)
        {
            _viewService.Show<ButtonsView>();
            _viewService.Hide<SimplePopupView>();
        }
    }
}