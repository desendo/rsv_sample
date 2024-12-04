using UnityEngine.EventSystems;

namespace Game.Views
{
    public interface ISelectableView : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
    }
}