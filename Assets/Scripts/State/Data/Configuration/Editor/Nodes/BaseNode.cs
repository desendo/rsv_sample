using System;
using System.Collections.Generic;
using System.Linq;
using Game.State.Data.Configuration.Editor.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using LanguageLocalizationAsset = Game.LocalizationAsset;

public class BaseNode : Node
{
    private readonly VisualElement _autocompleteContainer;
    private readonly Label _descriptionLabel;
    private readonly VisualElement _formContainer;
    private readonly LanguageLocalizationAsset _localizationAsset;
    private readonly VisualElement _rootGraphView;
    private readonly Button _selectButton;

    private readonly TextField _textField;
    private ListView _autocompleteListView;
    private bool _autocompleteVisible;
    private string _value;

    public BaseNode(LanguageLocalizationAsset localizationAsset, VisualElement rootGraphView)
    {
        _localizationAsset = localizationAsset;
        _rootGraphView = rootGraphView;
        style.width = 200;
        _textField = new TextField("Value");
        _textField.RegisterValueChangedCallback(evt =>
        {
            Value = evt.newValue; // Сохраняем значение
            UpdateAutocomplete(evt.newValue); // Обновляем окно автодополнения
        });
        _textField.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (_autocompleteVisible && evt.keyCode == KeyCode.Return)
            {
                SelectFirstAutocompleteItem();
                evt.StopPropagation();
            }
        });
        mainContainer.Add(_textField);
        // Контейнер для автодополнения
        _autocompleteContainer = new VisualElement
        {
            style =
            {
                position = Position.Absolute,
                backgroundColor = new Color(0, 0, 0, 0.9f),
                color = Color.white,
                borderTopLeftRadius = 4,
                borderTopRightRadius = 4,
                borderBottomLeftRadius = 4,
                borderBottomRightRadius = 4,
                borderBottomWidth = 1,
                borderTopWidth = 1,
                borderLeftWidth = 1,
                borderRightWidth = 1,
                //borderColor = Color.gray,
                maxHeight = 150,
                overflow = Overflow.Hidden,
                display = DisplayStyle.None
            }
        };

        _formContainer = new VisualElement
        {
            style =
            {
                position = Position.Absolute,
                backgroundColor = new Color(0, 0, 0, 0.9f),
                color = Color.white,
                borderTopLeftRadius = 4,
                borderTopRightRadius = 4,
                borderBottomLeftRadius = 4,
                borderBottomRightRadius = 4,
                paddingLeft = 10,
                paddingRight = 10,
                paddingTop = 10,
                paddingBottom = 10,
                display = DisplayStyle.None
            }
        };
        _rootGraphView.Add(_formContainer);
        _rootGraphView.Add(_autocompleteContainer);
        // Кнопка для выбора строки
        _selectButton = new Button(ShowDropdown)
        {
            text = "●",
            style = { width = 20, marginLeft = 5 }
        };
        _textField.parent.Add(_selectButton);

        // Лейбл для отображения значения
        _descriptionLabel = new Label();
        mainContainer.Add(_descriptionLabel);
    }

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            _textField.SetValueWithoutNotify(value);
            UpdateDescription();
        }
    }


    private void ShowDropdown()
    {
        var dropdownMenu = new GenericMenu();
        var firstLanguage = _localizationAsset.Languages.FirstOrDefault();
        if (firstLanguage == null)
            return;

        foreach (var entry in firstLanguage.Strings)
            dropdownMenu.AddItem(new GUIContent($"{entry.Id}: {entry.Value}"), false, () =>
            {
                Value = entry.Id;
                UpdateDescription();
            });

        dropdownMenu.ShowAsContext();
    }

    private void AddNewLocalizationEntry(string id, string value)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(value))
        {
            Debug.LogError("Id or Value cannot be empty!");
            return;
        }

        // Добавляем новую строку в локализацию
        _localizationAsset.Languages[0].Strings.Add(new LanguageLocalizationAsset.IdString
        {
            Id = id,
            Value = value
        });

        Debug.Log($"Added new localization entry: {id} - {value}");

        // Обновляем описание в ноде
        Value = id;
        UpdateDescription();
    }

    private void ShowAddNewValueForm(string newId)
    {
        // Создаём форму добавления нового значения


        // Поле для ввода ID
        var idField = new TextField("Id")
        {
            value = newId // Устанавливаем введённое значение
        };

        // Поле для ввода значения
        var valueField = new TextField("Value")
        {
            value = "" // Оставляем пустым для ввода нового значения
        };

        // Кнопка для добавления нового значения
        var addButton = new Button(() =>
        {
            AddNewLocalizationEntry(idField.value, valueField.value);
            _formContainer.style.display = DisplayStyle.None;
        })
        {
            text = "Add"
        };

        // Кнопка для закрытия формы без добавления
        var cancelButton = new Button(() => { _formContainer.style.display = DisplayStyle.None; })
        {
            text = "Cancel"
        };
        _formContainer.style.display = DisplayStyle.Flex;

        _formContainer.Clear();

        // Добавляем элементы в форму
        _formContainer.Add(new Label("Add New Localization Entry"));
        _formContainer.Add(idField);
        _formContainer.Add(valueField);
        _formContainer.Add(addButton);
        _formContainer.Add(cancelButton);


        // Позиционируем форму рядом с текстовым полем
        var textFieldWorldPos = _textField.worldBound;
        var graphViewPos = _rootGraphView.worldBound.position;

        _formContainer.style.left = textFieldWorldPos.x - graphViewPos.x;
        _formContainer.style.top = textFieldWorldPos.y - graphViewPos.y + _textField.resolvedStyle.height + 10;
        _formContainer.style.width = 250;
    }

    private void UpdateDescription()
    {
        var firstLanguage = _localizationAsset.Languages.FirstOrDefault();
        if (firstLanguage == null) return;

        var entry = firstLanguage.Strings.FirstOrDefault(s => s.Id == _value);
        _descriptionLabel.text = entry?.Value ?? "Value not found";
    }

    public List<int> GetChildrenNodesIndexes()
    {
        var indexes = new List<int>();
        foreach (var visualElement in outputContainer.Children())
            if (visualElement is Port port)
            {
                VisualElement p = parent;

                // Ищем родительский GraphView в иерархии
                while (p != null)
                {
                    if (p is GraphView graphView)
                    {
                        indexes.AddRange(port.GetConnectedOutputNodes(graphView));
                        break;
                    }

                    p = p.parent;
                }
            }

        return indexes;
    }

    public List<int> GetConnectedParentNodes()
    {
        var indexes = new List<int>();

        foreach (var visualElement in inputContainer.Children())
            if (visualElement is Port port)
            {
                VisualElement p = parent;

                // Ищем родительский GraphView в иерархии
                while (p != null)
                {
                    if (p is GraphView graphView)
                    {
                        indexes.AddRange(port.GetConnectedInputNodes(graphView));
                        break;
                    }

                    p = p.parent;
                }
            }

        return indexes;
    }

    private void UpdateAutocomplete(string input)
    {
        if (_localizationAsset == null) return;

        var firstLanguage = _localizationAsset.Languages.FirstOrDefault();
        if (firstLanguage == null) return;

        var matchingEntries = firstLanguage.Strings
            .Where(s => s.Id.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                        s.Value.Contains(input, StringComparison.OrdinalIgnoreCase))
            .ToList();

        _formContainer.style.display = DisplayStyle.None;
        if (matchingEntries.Any() && !string.IsNullOrEmpty(input))
        {
            // Показываем автодополнение
            ShowAutocomplete(matchingEntries);
        }
        else
        {
            CloseAutocomplete();

            if (input != null && !string.IsNullOrWhiteSpace(input)) ShowAddNewValueForm(input);
        }
    }

    private void HideAutocomplete()
    {
        _autocompleteContainer.Clear();

        _autocompleteContainer.style.display = DisplayStyle.None;
    }

    private void ShowAutocomplete(List<LanguageLocalizationAsset.IdString> entries)
    {
        if (_autocompleteContainer == null) return;

        // Очищаем контейнер
        _autocompleteContainer.Clear();

        // Создаём ListView
        _autocompleteListView = new ListView(entries, 20, () => new Label(), (element, i) =>
        {
            var entry = entries[i];
            (element as Label).text = $"{entry.Id}: {entry.Value}";
        });

        _autocompleteListView.selectionType = SelectionType.Single;
        _autocompleteListView.style.flexGrow = 1;

        _autocompleteListView.selectionChanged += OnAutocompleteListViewOnselectionChanged;

        _autocompleteContainer.Add(_autocompleteListView);

        // Определяем позицию текстового поля относительно GraphView
        var textFieldWorldPos = _textField.worldBound;
        var graphViewPos = _rootGraphView.worldBound.position;

        // Устанавливаем позицию контейнера
        _autocompleteContainer.style.left = textFieldWorldPos.x - graphViewPos.x;
        _autocompleteContainer.style.top = textFieldWorldPos.y - graphViewPos.y + _textField.resolvedStyle.height;
        _autocompleteContainer.style.width = _textField.resolvedStyle.width;
        _autocompleteContainer.style.display = DisplayStyle.Flex;
    }

    private void OnAutocompleteListViewOnselectionChanged(IEnumerable<object> selectedItems)
    {
        if (selectedItems.FirstOrDefault() is LanguageLocalizationAsset.IdString selectedEntry)
        {
            Value = selectedEntry.Id;
            CloseAutocomplete();
        }
    }

    private void CloseAutocomplete()
    {
        if (_autocompleteContainer == null)
            return;

        HideAutocomplete();
    }

    private void SelectFirstAutocompleteItem()
    {
        if (_autocompleteListView != null && _autocompleteListView.itemsSource.Count > 0)
            _autocompleteListView.selectedIndex = 0;
        CloseAutocomplete();
    }
}