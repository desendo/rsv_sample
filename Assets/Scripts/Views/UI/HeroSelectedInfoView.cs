using Game.Services;
using UnityEngine;

namespace Game.Views.UI
{
    public class HeroSelectedInfoView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        private HeroService _heroService;

        private void Awake()
        {
            _heroService = Di.Instance.Get<HeroService>();
            _heroService.Hero.Selected.Subscribe(x => _panel.SetActive(x));
            _panel.SetActive(false);
        }
    }
}