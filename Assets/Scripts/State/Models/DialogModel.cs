using Modules.Reactive.Values;

namespace Game.State.Models
{
    public class DialogModel : IModel
    {
        public string ConfigId { get; set; }

        public ReactiveVariable<int> CurrentNodeId { get; } = new();
        public ReactiveCollection<string> AnswersStringIds { get; } = new();
        public ReactiveVariable<string> QuestionStringId { get; } = new();
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; }


        public void Init(DialogConfig config)
        {
            ConfigId = config.Id;
        }
    }
}