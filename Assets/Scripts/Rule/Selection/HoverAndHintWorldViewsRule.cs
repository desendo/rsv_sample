using System;
using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class HoverAndHintWorldViewsRule
    {
        private readonly CameraService _cameraService;
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;
        private readonly HintService _hintService;
        private readonly List<IModelEnum<ISelectableModel>> _selectableServices;
        private readonly IUpdateProvider _updateProvider;
        private IDisposable _hintDelayProcedure;


        public HoverAndHintWorldViewsRule(ISignalBus signalBus, CameraService cameraService,
            HeroService heroService, GameConfig gameConfig, IUpdateProvider updateProvider,
            List<IModelEnum<ISelectableModel>> selectableServices, HintService hintService)
        {
            _cameraService = cameraService;
            _heroService = heroService;
            _gameConfig = gameConfig;
            _updateProvider = updateProvider;
            _selectableServices = selectableServices;
            _hintService = hintService;
            signalBus.Subscribe<WorldViewSignals.HoverRequest>(HandleHoverRequest);
            signalBus.Subscribe<WorldViewSignals.UnHoverRequest>(HandleUnHoverRequest);
            signalBus.Subscribe<UIViewSignals.HintRequest>(HandleHintRequest);
            signalBus.Subscribe<UIViewSignals.UnHintRequest>(HandleUnHintRequest);
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

        private void HandleUnHintRequest(UIViewSignals.UnHintRequest obj)
        {
            _hintDelayProcedure?.Dispose();
            _hintService.Reset();
        }


        private void HandleUnHoverRequest(WorldViewSignals.UnHoverRequest unHoverRequest)
        {
            if (unHoverRequest.Model is not ISelectableModel selectableModel)
                return;

            _hintDelayProcedure?.Dispose();
            _hintService.Reset();
            selectableModel.Hovered.Value = false;
        }

        private void HandleHintRequest(UIViewSignals.HintRequest obj)
        {
            Hint(obj.Model);
        }

        private void HandleHoverRequest(WorldViewSignals.HoverRequest hoverRequest)
        {
            if (hoverRequest.Model is ISelectableModel selectableModel) selectableModel.Hovered.Value = true;

            Hint(hoverRequest.Model);
        }

        //todo переделать хинты (или нет) 
        private void Hint(IModel model)
        {
            _hintService.HintShowDelay = _gameConfig.HintShowDelay;
            _hintService.HintHideDelay = _gameConfig.HintHideDelay;

            Action showAction = () =>
            {
                var hintText = "";
                var hintHeaderText = "";

                if (model is StorageItemModel storageItemModel)
                {
                    hintHeaderText = "Лкм";
                    hintText = $"выбросить {_gameConfig.Localization.GetObjectTitle(model.TypeId.Value)}";
                }

                if (model is WorldItemModel worldItemModel)
                {
                    if (_heroService.Hero.Selected.Value)
                    {
                        hintText = _gameConfig.Localization.GetObjectAction(model.TypeId.Value);
                        hintHeaderText = "Пкм";
                    }
                    else
                    {
                        hintText = _gameConfig.Localization.GetObjectTitle(model.TypeId.Value);
                    }
                }


                if (!string.IsNullOrEmpty(hintText))
                {
                    _hintService.HintShown.Value = true;
                    _hintService.HintHeader.Value = hintHeaderText;
                    _hintService.HintText.Value = hintText;
                    _hintService.HintScreenPosition.Value = Input.mousePosition;
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
                if (_hintService.IsTimeToShow)
                    showAction.Invoke();
                if (_hintService.IsTimeToHide)
                    hideAction.Invoke();
            });
        }

        private Vector3 GetHintScreenPosition(IModel model)
        {
            if (model is IWorldModel worldModel)
                return RectTransformUtility.WorldToScreenPoint(_cameraService.Camera.Value, worldModel.Position.Value);
            return Vector3.zero;
        }

        private void HintDelayProcedure(float dt, Action callback)
        {
            _hintService.AddTime(dt);
            if (_hintService.IsTimeToShow)
                callback.Invoke();
        }

        private void DeselectAll()
        {
            foreach (var selectableService in _selectableServices)
            foreach (var selectableModel in selectableService.GetEnum())
                selectableModel.Selected.Value = false;
        }
    }
}