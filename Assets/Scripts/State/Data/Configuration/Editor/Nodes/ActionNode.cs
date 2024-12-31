using UnityEditor.Experimental.GraphView;

public class ActionNode : Node, IChoiceResult
{
    public ActionNode(NodeGraphView nodeGraphView)
    {
        var inputPortConditions = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
            typeof(ChoiceNode));
        inputPortConditions.portName = "Действие";
        inputContainer.Add(inputPortConditions);
    }
}

public interface IChoiceResult
{
}