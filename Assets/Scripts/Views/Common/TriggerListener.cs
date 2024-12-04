using Modules.Reactive.Actions;
using UnityEngine;

namespace Game.Views.Common
{
    public class TriggerListener : MonoBehaviour
    {
        public IReactiveEvent<Collider2D> Other { get; } = new ReactiveEvent<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            Other.Invoke(other);
        }
    }
}