using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class GroundView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _wayPointMarker;
        public Plane Plane { get; } = new Plane(Vector3.up, Vector3.zero);

        private HeroModel _hero;

        private void Awake()
        {

            _hero = Di.Instance.Get<HeroService>().Hero;
            _hero.HasWayPoint.Subscribe(b => UpdateMarker());
            _hero.Selected.Subscribe(b => UpdateMarker());
            _hero.WayPoint.Subscribe(b => UpdateMarker());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Di.Instance.Get<ISignalBus>()
                .Fire(new WorldViewSignals.GroundClick(eventData.button,
                    eventData.pointerCurrentRaycast.worldPosition));
        }

        private void UpdateMarker()
        {
            _wayPointMarker.transform.position = _hero.WayPoint.Value;
            _wayPointMarker.SetActive(_hero.Selected.Value && _hero.HasWayPoint.Value);
        }
    }
}