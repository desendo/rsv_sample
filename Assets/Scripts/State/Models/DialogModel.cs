using Modules.Reactive.Values;

namespace Game.State.Models
{
    public class DialogModel : IModel
    {
        public string ConfigId { get; set; }
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; }

        public ReactiveVariable<int> CurrentNodeId { get; } = new ReactiveVariable<int>();
        public ReactiveCollection<string> AnswersStringIds { get; } = new ReactiveCollection<string>();
        public ReactiveVariable<string> QuestionStringId { get; } = new ReactiveVariable<string>();


        public void Init(DialogConfig config)
        {
            ConfigId = config.Id;
            
        }


    }
}