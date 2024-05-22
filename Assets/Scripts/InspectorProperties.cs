using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIManager _uiManager = (UIManager)target;
        
        if (GUILayout.Button("Set Quality"))
        {
            _uiManager.SetQuality();
        }
    }
}