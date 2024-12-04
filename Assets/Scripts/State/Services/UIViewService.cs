using System;
using System.Collections.Generic;

namespace Game.Services
{
    public class UIViewService : IUIViewService
    {
        private readonly List<object> _allViews = new();
        public event Action<IUIView> OnViewShown;
        public event Action<IUIView> OnViewHidden;

        public void RegisterView(IUIView view)
        {
            _allViews.Add(view);
        }

        public void Show<T>() where T : IUIView
        {
            foreach (var obj in _allViews)
                if (obj is T view)
                {
                    view.Show();
                    OnViewShown?.Invoke(view);
                }
        }

        public void Show<T, T1>(T1 data) where T : IUIView<T1>
        {
            foreach (var obj in _allViews)
                if (obj is T view)
                {
                    view.Show(data);
                    OnViewShown?.Invoke(view);
                }
        }

        public void Hide<T>() where T : IUIView
        {
            foreach (var obj in _allViews)
                if (obj is T view)
                {
                    view.Hide();
                    OnViewHidden?.Invoke(view);
                }
        }

        public void CloseAll()
        {
        }
    }

    public interface IUIView<in T> : IUIView
    {
        public void Show(T data);
    }

    public interface IUIView
    {
        public void Show();
        public void Hide();
    }

    public interface IUIViewService
    {
        event Action<IUIView> OnViewShown;
        event Action<IUIView> OnViewHidden;
        public void RegisterView(IUIView view);
        public void Show<T>() where T : IUIView;
        public void Show<T, T1>(T1 data) where T : IUIView<T1>;
        public void Hide<T>() where T : IUIView;
        public void CloseAll();
    }
}