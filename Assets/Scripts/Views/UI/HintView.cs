using Game.Services;
using TMPro;
using UnityEngine;

namespace Game.Views.UI
{
    public class HintView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            var service = Di.Instance.Get<HintService>();
            service.HintText.Subscribe(text => _text.text = text);
            service.HintShown.Subscribe(_panel.SetActive);
        }
    }
}