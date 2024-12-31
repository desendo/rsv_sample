using Game.Services.ServicesBase;
using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Actions;
using Modules.Reactive.Values;

namespace Game.Services
{
    public class DialogsService : InventoryServiceBase<DialogModel>, ISaveLoadData<StateData>
    {
        private readonly IReactiveVariable<DialogModel> _active = new ReactiveVariable<DialogModel>();
        public IReadOnlyReactiveVariable<DialogModel> ActiveDialog => _active;
        public IReactiveEvent<string, int> StartDialogRequest { get; } = new ReactiveEvent<string, int>();

        public void LoadFrom(in StateData data)
        {
        }

        public void SaveTo(StateData data)
        {
        }

        public void SetActiveDialog(DialogModel model)
        {
            _active.Value = model;
        }


        public void StopDialog()
        {
            _active.Value = null;
        }
    }
}