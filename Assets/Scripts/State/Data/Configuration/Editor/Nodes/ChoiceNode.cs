using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using LocalizationAsset = Game.LocalizationAsset;

public sealed class ChoiceNode : BaseNode
{
    public ChoiceNode(LocalizationAsset localizationAsset, VisualElement rootGraphView) : base(localizationAsset,
        rootGraphView)
    {
        this.title = "Ответ";
        style.width = 200;
        style.backgroundColor = new StyleColor(Color.blue);

        var outPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
            typeof(IChoiceResult));
        outPort.portName = "Результат";
        outputContainer.Add(outPort);

        var inputPortConditions = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
            typeof(ConditionNode));
        inputPortConditions.portName = "Условия";
        inputContainer.Add(inputPortConditions);

        var inputPortQuestion = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
            typeof(DialogNode));
        inputPortQuestion.portName = "Вопрос";
        inputContainer.Add(inputPortQuestion);


        RefreshExpandedState();
        RefreshPorts();
    }
}