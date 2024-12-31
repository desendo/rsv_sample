#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using LocalizationAsset = Game.LocalizationAsset;

public class NodeGraphView : GraphView
{
    private readonly DialogConfigAsset _dialogConfigAsset;
    private readonly LocalizationAsset _localizationAsset;

    public NodeGraphView(DialogConfigAsset dialogConfigAsset, LocalizationAsset localizationAsset)
    {
        _dialogConfigAsset = dialogConfigAsset;
        _localizationAsset = localizationAsset;
        graphViewChanged = OnGraphViewChanged;

        // Добавляем управление масштабированием и панорамированием
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        // Добавляем рамку
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var gridBackground = new GridBackground();

        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();
        this.StretchToParentSize();
    }

    public List<DialogNode> GetDialogNodes => nodes.ToList().OfType<DialogNode>().ToList();
    public List<Edge> GetEdges => edges.ToList();

    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        if (change.edgesToCreate != null)
            foreach (var edge in change.edgesToCreate)
            {
                var inputNode = edge.input?.node as DialogNode;
                var outputNode = edge.output?.node as DialogNode;

                if (inputNode != null && outputNode != null)
                {
                }
            }

        // Обработка удалённых элементов (например, связей)
        if (change.elementsToRemove != null)
            foreach (var element in change.elementsToRemove)
                if (element is Edge edge)
                {
                    var inputNode = edge.input?.node as DialogNode;
                    var outputNode = edge.output?.node as DialogNode;

                    if (inputNode != null && outputNode != null)
                    {
                        //inputNode.ChildrenIds.Remove(outputNode.NodeId);
                    }
                }

        return change;
    }


    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()!.Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node &&
                startPort.portType == endPort.node.GetType() &&
                endPort.portType == startPort.node.GetType())
            .ToList();
    }

    public DialogNode CreateDialogNode(Vector2 position)
    {
        var node = new DialogNode(_localizationAsset, this);
        node.SetPosition(new Rect(position, new Vector2(200, 150)));

        return node;
    }

    public ConditionNode CreateConditionNode(Vector2 position)
    {
        var node = new ConditionNode(_localizationAsset, this);
        node.SetPosition(new Rect(position, new Vector2(200, 150)));

        return node;
    }

    public ChoiceNode CreateChoiceNode(Vector2 position)
    {
        var node = new ChoiceNode(_localizationAsset, this);
        node.SetPosition(new Rect(position, new Vector2(200, 150)));

        return node;
    }
    public ActionNode CreateActionNode(Vector2 position)
    {
        var node = new ActionNode( this);
        node.SetPosition(new Rect(position, new Vector2(200, 150)));

        return node;
    }

    public void AddNewConditionNode()
    {
        var node = CreateConditionNode(new Vector2(200, 200));
        AddElement(node);
    }

    public void AddNewDialogNode()
    {
        var node = CreateDialogNode(new Vector2(200, 200));
        AddElement(node);
    }

    public void AddNewChoiceNode()
    {
        var node = CreateChoiceNode(new Vector2(200, 200));
        AddElement(node);
    }
}
#endif