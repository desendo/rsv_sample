using Game;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public sealed class DialogNode : BaseNode, IChoiceResult
{
    public DialogNode(LocalizationAsset localizationAsset, VisualElement rootGraphView) : base(localizationAsset,
        rootGraphView)
    {
        title = "Вопрос";

        style.width = 200;

        var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
            typeof(ChoiceNode));
        inputPort.portName = "Причина";
        inputContainer.Add(inputPort);

        var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
            typeof(ChoiceNode));
        outputPort.portName = "Вариант";
        outputContainer.Add(outputPort);


        RefreshExpandedState();
        RefreshPorts();
    }
}