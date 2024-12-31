using System.Collections.Generic;
using Game.Services;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HeroSelectedInfoView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private List<ParameterView> _parameterViews;
        [SerializeField] private TextMeshProUGUI _speechLog;
        private HeroService _heroService;

        private void Awake()
        {
            foreach (var parameterView in _parameterViews) parameterView.gameObject.SetActive(false);
            _heroService = Di.Instance.Get<HeroService>();
            _heroService.Hero.Selected.Subscribe(x => _panel.SetActive(x));
            _heroService.Hero.Speech.Subscribe(s => _speechLog.text = s);
            _parameterViews[0].gameObject.SetActive(true);
            _parameterViews[0].Bind(_heroService.HeroParameters.HungerParameter.Title,
                _heroService.HeroParameters.HungerParameter.ValueString,
                _heroService.HeroParameters.HungerParameter.Normalized);

            _parameterViews[1].gameObject.SetActive(true);
            _parameterViews[1].Bind(_heroService.HeroParameters.MassParameter.Title,
                _heroService.HeroParameters.MassParameter.ValueString,
                _heroService.HeroParameters.MassParameter.Normalized);

            _panel.SetActive(false);
        }
    }
}