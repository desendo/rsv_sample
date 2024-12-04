using System;
namespace Modules.Common
{
    public class DisposeContainer : IDisposable
    {
        private Action disposeAction;

        public DisposeContainer(Action disposeAction)
        {
            this.disposeAction = disposeAction;
        }
        public DisposeContainer()
        {
        }
        public void SetDisposeAction(Action action)
        {
            this.disposeAction = action;
        }

        public void Dispose()
        {
            this.disposeAction?.Invoke();
        }
    }
}