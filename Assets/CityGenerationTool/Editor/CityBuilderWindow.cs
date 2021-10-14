using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityBuilderWindow : EditorWindow
{
    int gridInt = 0;
    string[] brushNames = { "Down Town", "CBD", "Residential", "Industrial", "Ghetto", "Green Zone" };

    [MenuItem("Tools/City Builder Window")]   
    public static void ShowWindow()
    {
        GetWindow(typeof(CityBuilderWindow)); // showing the editor window
    }

    private void OnGUI() 
    {
        GUILayout.Label("Select Brush", EditorStyles.boldLabel);
        gridInt = GUILayout.SelectionGrid(gridInt, brushNames, 2);


    }
}
