using Game.State.Models;
using Modules.Reactive.Values;

namespace Game.Services
{
    public class HintService : IReset
    {
        public IReactiveVariable<bool> HintShown = new ReactiveVariable<bool>();
        public IReactiveVariable<string> HintText = new ReactiveVariable<string>();

        public void Reset()
        {
            HintText.Value = "";
            HintShown.Value = false;
        }
    }
}