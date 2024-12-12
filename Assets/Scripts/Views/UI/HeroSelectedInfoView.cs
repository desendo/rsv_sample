using System.Collections.Generic;
using Game.Services;
using Modules.Reactive.Values;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HeroSelectedInfoView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private List<ParameterView> _parameterViews;
        [SerializeField] private TextMeshProUGUI _speechLog;
        private UnitsService _unitsService;

        private void Awake()
        {
            foreach (var parameterView in _parameterViews)
            {
                parameterView.gameObject.SetActive(false);
            }
            _unitsService = Di.Instance.Get<UnitsService>();
            _unitsService.Hero.Selected.Subscribe(x => _panel.SetActive(x));
            _unitsService.Hero.Speech.Subscribe(s => _speechLog.text = s);
            _parameterViews[0].gameObject.SetActive(true);
            _parameterViews[0].Bind(_unitsService.HeroParameters.HungerParameter.Title,
                _unitsService.HeroParameters.HungerParameter.ValueString,
                _unitsService.HeroParameters.HungerParameter.Normalized);

            _parameterViews[1].gameObject.SetActive(true);
            _parameterViews[1].Bind(_unitsService.HeroParameters.MassParameter.Title,
                _unitsService.HeroParameters.MassParameter.ValueString,
                _unitsService.HeroParameters.MassParameter.Normalized);

            _panel.SetActive(false);
        }
    }
}