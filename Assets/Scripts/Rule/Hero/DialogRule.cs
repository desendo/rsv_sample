using System.Linq;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class DialogRule
    {
        private readonly DialogsService _dialogsService;
        private readonly HeroService _heroService;
        private readonly SignalBus _signalBus;

        public DialogRule(DialogsService dialogsService, SignalBus signalBus, HeroService heroService)
        {
            _dialogsService = dialogsService;
            _dialogsService.StartDialogRequest.Subscribe(HandleStartDialogRequest);
            _signalBus = signalBus;
            _signalBus.Subscribe<UIViewSignals.DialogOptionRequest>(HandleOptionRequest);
            _heroService = heroService;
            _heroService.Hero.WayPoint.Subscribe(w => { _dialogsService.StopDialog(); });
            _signalBus.Subscribe<UIViewSignals.CancelDialogRequest>(HandleCancelDialogRequest);
        }

        private void HandleOptionRequest(UIViewSignals.DialogOptionRequest obj)
        {
            Debug.Log("option " + obj.Index);
        }

        private void HandleStartDialogRequest(string configId, int modelUId)
        {
            DialogModel dialog = null;
            foreach (var model in _dialogsService)
                if (model.ConfigId == configId && model.UId == modelUId)
                {
                    dialog = model;
                    break;
                }

            if (dialog == null)
            {
                var config = Di.Instance.Get<GameConfig>().DialogConfigAsset.DialogConfigs
                    .FirstOrDefault(x => x.Id == configId);
                dialog = new DialogModel();
                dialog.Init(config);
                _dialogsService.Add(dialog);
            }


            _dialogsService.SetActiveDialog(dialog);
        }

        private void HandleCancelDialogRequest(UIViewSignals.CancelDialogRequest obj)
        {
            _dialogsService.StopDialog();
        }
    }
}