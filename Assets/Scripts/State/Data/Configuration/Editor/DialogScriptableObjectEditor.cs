using Game;
using UnityEditor;
using UnityEngine;
using LocalizationAsset = Game.LocalizationAsset;

#if UNITY_EDITOR
[CustomEditor(typeof(DialogConfigAsset))]
public class DialogScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var scriptableObject = (DialogConfigAsset)target;
        var dialogLocalization = Resources.Load<LocalizationAsset>("DialogsLocalizationAsset");

        for (var i = 0; i < scriptableObject.DialogConfigs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"Dialog {i + 1} id: {scriptableObject.DialogConfigs[i].Id}",
                GUILayout.Width(200));

            if (GUILayout.Button("Open Dialog Editor", GUILayout.Width(150)))
                NodeEditorWindow.OpenEditor(scriptableObject.DialogConfigs[i], scriptableObject, dialogLocalization);

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif