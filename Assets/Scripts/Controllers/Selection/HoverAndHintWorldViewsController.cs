using System;
using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Controllers
{
    public class HoverAndHintWorldViewsController
    {
        private readonly GameConfig _gameConfig;
        private readonly IUpdateProvider _updateProvider;
        private readonly CameraService _cameraService;
        private readonly HeroService _heroService;
        private readonly HintService _hintService;
        private readonly List<IModelList<ISelectableModel>> _selectableServices;
        private IDisposable _hintDelayProcedure;


        public HoverAndHintWorldViewsController(ISignalBus signalBus, CameraService cameraService,
            HeroService heroService, GameConfig gameConfig, IUpdateProvider updateProvider,
            List<IModelList<ISelectableModel>> selectableServices, HintService hintService)
        {
            _cameraService = cameraService;
            _heroService = heroService;
            _gameConfig = gameConfig;
            _updateProvider = updateProvider;
            _selectableServices = selectableServices;
            _hintService = hintService;
            signalBus.Subscribe<WorldViewSignals.HoverRequest>(HandleHoverRequest);
            signalBus.Subscribe<WorldViewSignals.UnHoverRequest>(HandleUnHoverRequest);
            signalBus.Subscribe<WorldViewSignals.ActionRequest>(request =>
            {
                _hintDelayProcedure?.Dispose();
                _hintService.Reset();
            });
            signalBus.Subscribe<WorldViewSignals.SelectRequest>(request =>
            {
                _hintDelayProcedure?.Dispose();
                _hintService.Reset();
            });
        }


        private void HandleUnHoverRequest(WorldViewSignals.UnHoverRequest unHoverRequest)
        {
            _hintDelayProcedure?.Dispose();
            _hintService.Reset();
            unHoverRequest.Model.Hovered.Value = false;
        }

        private void HandleHoverRequest(WorldViewSignals.HoverRequest hoverRequest)
        {
            hoverRequest.Model.Hovered.Value = true;
            if (hoverRequest.Model is not IModel model)
                return;

            _hintService.HintShowDelay = _gameConfig.HintShowDelay;
            _hintService.HintHideDelay = _gameConfig.HintHideDelay;

            Action showAction = () =>
            {
                var hintText = "";
                var hintHeaderText = "";

                if (_heroService.Hero.Selected.Value)
                {
                    hintText = _gameConfig.Localization.GetObjectAction(model.TypeId.Value);
                    hintHeaderText = "пкм";
                }
                else
                    hintText = _gameConfig.Localization.GetObjectTitle(model.TypeId.Value);

                if (!string.IsNullOrEmpty(hintText))
                {
                    _hintService.HintShown.Value = true;
                    _hintService.HintHeader.Value = hintHeaderText;
                    _hintService.HintText.Value = hintText;
                    _hintService.HintScreenPosition.Value = GetHintScreenPosition(model);
                }
            };
            Action hideAction = () =>
            {
                _hintService.Reset();
                _hintDelayProcedure?.Dispose();
            };
            _hintDelayProcedure?.Dispose();
            _hintService.Reset();
            _hintDelayProcedure = _updateProvider.OnTick.Subscribe(f =>
            {
                _hintService.AddTime(f);
                if(_hintService.IsTimeToShow)
                    showAction.Invoke();
                if(_hintService.IsTimeToHide)
                    hideAction.Invoke();
            });

        }

        private Vector3 GetHintScreenPosition(IModel model)
        {
            if (model is IWorldModel worldModel)
            {
                return RectTransformUtility.WorldToScreenPoint(_cameraService.Camera.Value, worldModel.Position.Value);
            }
            return Vector3.zero;
        }

        private void HintDelayProcedure(float dt, Action callback)
        {
            _hintService.AddTime(dt);
            if(_hintService.IsTimeToShow)
                callback.Invoke();
        }

        private void DeselectAll()
        {
            foreach (var selectableService in _selectableServices)
            foreach (var selectableModel in selectableService.GetList())
                selectableModel.Selected.Value = false;
        }
    }
}