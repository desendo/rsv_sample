using System;
using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using Modules.Reactive.Values;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class DialogView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Transform _answersParent;
        [SerializeField] private GameObject _answerPrefab;
        [SerializeField] private List<GameObject> _answerInstances;
        [SerializeField] private TextMeshProUGUI _question;
        private ISignalBus _signalBus;
        private DialogsService _dialogService;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        private void Awake()
        {
            _dialogService = Di.Instance.Get<DialogsService>();
            _dialogService.ActiveDialog.Subscribe(HandleActiveDialog);
            _signalBus = Di.Instance.Get<ISignalBus>();
            _cancelButton.onClick.AddListener(() => _signalBus.Fire(new UIViewSignals.CancelDialogRequest()));
            _answerPrefab.SetActive(false);
        }



        private void HandleActiveDialog(DialogModel model)
        {
            _disposables?.DisposeAndClear();
            _panel.SetActive(model != null);
            if(model == null)
                return;

            model.AnswersStringIds.OnAnyEvent.Subscribe(UpdateAnswers).AddTo(_disposables);
            UpdateAnswers(model.AnswersStringIds);
            model.QuestionStringId.Subscribe(UpdateQuestion).AddTo(_disposables);

        }

        private void UpdateAnswers(ICollection<string> id)
        {
            foreach (var answerInstance in _answerInstances)
            {
                answerInstance.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Destroy(answerInstance);
            }
            _answerInstances.Clear();
            var list = id.ToList();
            for (var index = 0; index < list.Count; index++)
            {
                var s = list[index];
                var instance = GameObject.Instantiate(_answerPrefab, _answerPrefab.transform.parent);
                instance.GetComponent<TextMeshProUGUI>().text = LocService.ById(s).Value;
                var optionIndex = index;
                instance.GetComponent<Button>().onClick.AddListener(()=>_signalBus.Fire(new UIViewSignals.DialogOptionRequest(optionIndex)));
                _answerInstances.Add(instance);
            }
        }
        private void UpdateQuestion(string id)
        {
            if(string.IsNullOrEmpty(id))
                return;

            _question.text = LocService.ById(id).Value;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _signalBus.Fire(new UIViewSignals.CancelDialogRequest());
        }


    }
}