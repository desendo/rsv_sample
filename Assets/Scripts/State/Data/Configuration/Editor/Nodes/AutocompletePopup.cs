using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using LocalizationAsset = Game.LocalizationAsset;

public class AutocompletePopup : PopupWindowContent
{
    private readonly Action<LocalizationAsset.IdString> _onSelect;
    private List<LocalizationAsset.IdString> _entries;
    private Vector2 _scrollPosition;

    public AutocompletePopup(List<LocalizationAsset.IdString> entries, Action<LocalizationAsset.IdString> onSelect)
    {
        _entries = entries;
        _onSelect = onSelect;
    }

    public bool HasEntries => _entries != null && _entries.Any();

    public override Vector2 GetWindowSize()
    {
        return new Vector2(300, Mathf.Min(200, _entries.Count * 25 + 50));
    }

    public override void OnGUI(Rect rect)
    {
        EditorGUILayout.LabelField("Select an entry", EditorStyles.boldLabel);
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        foreach (var entry in _entries)
            if (GUILayout.Button($"{entry.Id}: {entry.Value}", EditorStyles.miniButton))
            {
                _onSelect?.Invoke(entry);
                editorWindow.Close();
            }

        EditorGUILayout.EndScrollView();
    }

    public void UpdateEntries(List<LocalizationAsset.IdString> newEntries)
    {
        _entries = newEntries;
        editorWindow.Repaint();
    }

    public void SelectFirstEntry()
    {
        if (_entries.Any()) _onSelect?.Invoke(_entries.First());
    }
}