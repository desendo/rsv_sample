//#if UNITY_EDITOR

using System.Linq;
using Game;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using LocalizationAsset = Game.LocalizationAsset;

public class NodeEditorWindow : EditorWindow
{
    private NodeGraphView _graphView;
    private DialogConfig _currentConfig;
    private DialogConfigAsset _dialogConfigAsset;
    private LocalizationAsset _localizationAsset;

    [MenuItem("Window/Node Editor")]
    public static void OpenWindow()
    {
        var dialogConfigAsset = Resources.Load<DialogConfigAsset>("Dialog config");
        var dialogLocalization = Resources.Load<LocalizationAsset>("DialogsLocalizationAsset");
        OpenEditor(new DialogConfig(), dialogConfigAsset, dialogLocalization);
    }

    private void Initialize()
    {
        _graphView = new NodeGraphView(_dialogConfigAsset, _localizationAsset);
        rootVisualElement.Clear();
        rootVisualElement.Add(_graphView);
        _graphView.StretchToParentSize();

        var toolbar = new Toolbar();
        rootVisualElement.Add(toolbar);

        toolbar.Add(new Button(_graphView.AddNewDialogNode) { text = "Add Dialog Node" });
        toolbar.Add(new Button(_graphView.AddNewChoiceNode) { text = "Add Choice Node" });
        toolbar.Add(new Button(_graphView.AddNewConditionNode) { text = "Add Condition Node" });
        toolbar.Add(new Button(SaveConfig) { text = "Save Config" });
    }

    private void Dispose()
    {
        //if (_graphView != null)
        _graphView?.Clear();
        rootVisualElement.Clear();
    }

    private void LoadConfig()
    {
        if (_currentConfig == null)
        {
            Debug.LogError("null _currentConfig");
            return;
        }

        if (_currentConfig.DialogNodes.Count == 0)
        {
            Debug.LogError("0 elements in _currentConfig");

            return;
        }

        foreach (var configNode in _currentConfig.DialogNodes)
        {

        }



            /*
            if (configNode?.ChildrenIds == null)
                continue;

            var outPort = node.outputContainer[0] as Port;

            foreach (var childrenId in configNode.ChildrenIds)
            {
                if (node.Id == childrenId)
                    continue;

                var id = childrenId;
                var childNode = nodes.FirstOrDefault(x => x.Id == id);
                if (childNode == null) throw new Exception("null node index " + id);

                var childNodeInputContainer = childNode.inputContainer;
                var inPort = childNodeInputContainer[0] as Port;
                if (inPort == null)
                {
                    Debug.LogWarning("inPort null");
                    continue;
                }

                if (outPort != null)
                {
                    var edge = outPort.ConnectTo(inPort);
                    _graphView.AddElement(edge);
                }
            }*/
        
    }


    private void SaveConfig()
    {
        if (_currentConfig == null)
        {
            Debug.LogError("_currentConfig null");
            return;
        }

        var nodes = _graphView.nodes.ToList().OfType<Node>().ToList();
        var index = 0;
        foreach (var node in _graphView.nodes)
        {

            if (node is DialogNode dialogNode)
            {

                _currentConfig.DialogNodes.Add(new DialogNodeConfig
                {
                    Value = dialogNode.Value,
                    Id = index,
                    Children = dialogNode.GetChildrenNodesIndexes(),
                    Parent = dialogNode.GetConnectedParentNodes(),
                    Position = dialogNode.GetPosition()
                });
            }
            if (node is ChoiceNode choiceNode)
            {

                _currentConfig.ChoiceNodes.Add(new ChoiceNodeConfig()
                {
                    Value = choiceNode.Value,
                    Id = index,
                    Children = choiceNode.GetChildrenNodesIndexes(),
                    Position = choiceNode.GetPosition(),
                    Parent = choiceNode.GetConnectedParentNodes()
                });
            }
            if (node is ConditionNode conditionNode)
            {

                _currentConfig.ConditionNodes.Add(new ConditionNodeConfig()
                {
                    Value = conditionNode.Value,
                    Id = index,
                    Children = conditionNode.GetChildrenNodesIndexes(),
                    Type = conditionNode.ConditionType,
                    Position = conditionNode.GetPosition()
                });
            }
            index++;
        }



        // Закрываем окно
        Close();
    }

    private void OnDisable()
    {
        Dispose();
        // Удаляем GraphView из окна при закрытии
    }


    public static void OpenEditor(DialogConfig dialogConfig, DialogConfigAsset dialogConfigAsset,
        LocalizationAsset dialogLocalization)
    {
        var window = GetWindow<NodeEditorWindow>();

        window.titleContent = new GUIContent("Node Editor");
        window._currentConfig = dialogConfig;
        window._dialogConfigAsset = dialogConfigAsset;
        window._localizationAsset = dialogLocalization;
        window.Initialize();
        window.LoadConfig();
    }
}
//#endif