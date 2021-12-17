#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DeckControllerHelper : MonoBehaviour
{
    public DeckController controller;
}

[CustomEditor(typeof(DeckControllerHelper))]
public class DeckControllerHelperEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        DeckControllerHelper manager = (DeckControllerHelper)target;

        manager.controller = (DeckController)EditorGUILayout.ObjectField(manager.controller, typeof(DeckController), true);
        
        if (GUILayout.Button("Init Deck"))
        {
            manager.controller.Init();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif