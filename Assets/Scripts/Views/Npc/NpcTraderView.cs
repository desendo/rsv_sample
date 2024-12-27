
using Game.Services;
using Game.Signals;
using Game.State.Data;
using Game.State.Models;
using Modules.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.Npc
{
    public class NpcTraderView : MonoBehaviour, ISelectableView, IView
    {
        [SerializeField] private GameObject _selectionMarker;
        [SerializeField] private GameObject _hoverMarker;
        [SerializeField] private Transform _3d;

        [SerializeField] private NpcModelData _data;
        private NpcService _npcService;

        private NpcModel _model;
        private void Awake()
        {
            _npcService = Di.Instance.Get<NpcService>();
            //создаем новую модель по данным из инспектора и биндим к ней вью
            // если модель уже существует, то возвращаем существующую и биндим к ней вью
            // если модель когда то существовала, но текущий стейт говорит что она уничтожена то возвращаем null
            var model = _npcService.TryCreateOrGetModelFromView(_data, this);

            BindModel(model);

        }

        private void BindModel(NpcModel model)
        {
            if (model == null)
            {
                //если модели нет, то выключаем вью
                gameObject.SetActive(false);
                return;
            }
            _model = model;
            _model.Hovered.Subscribe(SetHovered);
            _model.Selected.Subscribe(SetSelected);
            _model.Rotation.Subscribe(SetRotation);
            _model.Position.Subscribe(SetPosition);
        }

        //listen events and emit signals
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.SelectRequest(_model));
            if (eventData.button == PointerEventData.InputButton.Right)
                Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.ActionRequest(_model));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.HoverRequest(_model));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.UnHoverRequest(_model));
        }

        private void SetPosition(Vector3 obj)
        {
            transform.position = obj;
        }

        private void SetRotation(Quaternion obj)
        {
            transform.rotation = obj;
        }

        private void SetSelected(bool obj)
        {
            _selectionMarker.gameObject.SetActive(obj);
        }

        private void SetHovered(bool obj)
        {
            _hoverMarker.gameObject.SetActive(obj);
        }
    }
}
