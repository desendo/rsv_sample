﻿using System;
using System.Collections.Generic;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine.EventSystems;

namespace Game.Rules
{
    public class SelectWorldViewsRule
    {

        private readonly List<IModelList<ISelectableModel>> _selectableServices;
        private IDisposable _hintDelayProcedure;


        public SelectWorldViewsRule(ISignalBus signalBus, List<IModelList<ISelectableModel>> selectableServices)
        {
            _selectableServices = selectableServices;
            signalBus.Subscribe<WorldViewSignals.GroundClick>(HandleGroundClick);
            signalBus.Subscribe<WorldViewSignals.SelectRequest>(HandleSelectRequest);
        }

        private void HandleGroundClick(WorldViewSignals.GroundClick obj)
        {
            if (obj.Button == PointerEventData.InputButton.Left) DeselectAll();
        }

        private void HandleSelectRequest(WorldViewSignals.SelectRequest selectRequest)
        {
            DeselectAll();
            if (selectRequest.Model is ISelectableModel selectableModel)
            {
                selectableModel.Selected.Value = true;
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