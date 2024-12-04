using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine.EventSystems;

namespace Game.Controllers
{
    public class SelectWorldViewsController
    {
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;
        private readonly HintService _hintService;
        private readonly List<IModelList<ISelectableModel>> _selectableServices;

        public SelectWorldViewsController(ISignalBus signalBus,
            HeroService heroService, GameConfig gameConfig,
            List<IModelList<ISelectableModel>> selectableServices, HintService hintService)
        {
            _heroService = heroService;
            _gameConfig = gameConfig;
            _selectableServices = selectableServices;
            _hintService = hintService;
            signalBus.Subscribe<WorldViewSignals.GroundClick>(HandleGroundClick);
            signalBus.Subscribe<WorldViewSignals.HoverRequest>(HandleHoverRequest);
            signalBus.Subscribe<WorldViewSignals.UnHoverRequest>(HandleUnHoverRequest);
            signalBus.Subscribe<WorldViewSignals.SelectRequest>(HandleSelectRequest);
        }

        private void HandleGroundClick(WorldViewSignals.GroundClick obj)
        {
            if (obj.Button == PointerEventData.InputButton.Left) DeselectAll();
        }

        private void HandleSelectRequest(WorldViewSignals.SelectRequest selectRequest)
        {
            DeselectAll();
            selectRequest.Model.Selected.Value = true;
        }

        private void HandleUnHoverRequest(WorldViewSignals.UnHoverRequest unHoverRequest)
        {
            _hintService.HintShown.Value = false;
            unHoverRequest.Model.Hovered.Value = false;
        }

        private void HandleHoverRequest(WorldViewSignals.HoverRequest hoverRequest)
        {
            hoverRequest.Model.Hovered.Value = true;
            if (hoverRequest.Model is not IModel model)
                return;

            var hintText = "";

            if (_heroService.Hero.Selected.Value)
                hintText = _gameConfig.Localization.GetObjectAction(model.TypeId.Value);
            else
                hintText = _gameConfig.Localization.GetObjectTitle(model.TypeId.Value);



            if (!string.IsNullOrEmpty(hintText))
            {
                _hintService.HintShown.Value = true;
                _hintService.HintText.Value = hintText;
            }
        }

        private void DeselectAll()
        {
            foreach (var selectableService in _selectableServices)
            foreach (var selectableModel in selectableService.GetList())
                selectableModel.Selected.Value = false;
        }
    }
}