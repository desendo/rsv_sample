using System;
using System.Linq;
using Game;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using LocalizationAsset = Game.LocalizationAsset;

public sealed class ConditionNode : BaseNode
{
    private readonly DropdownField _type;
    public ConditionNodeType ConditionType;

    public ConditionNode(LocalizationAsset localizationAsset, VisualElement rootGraphView) : base(localizationAsset,
        rootGraphView)
    {
        style.backgroundColor = new StyleColor(Color.grey);
        this.title = "Условие ответа";

        var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
            typeof(ChoiceNode));

        outputPort.portName = "Ответ";
        outputContainer.Add(outputPort);
        var options = Enum.GetNames(typeof(ConditionNodeType)).ToList();
        _type = new DropdownField("Тип условия", options, 0);
        _type.RegisterValueChangedCallback(evt => { ConditionType = Enum.Parse<ConditionNodeType>(evt.newValue); });

        mainContainer.Add(_type);
        RefreshExpandedState();
        RefreshPorts();
    }
}