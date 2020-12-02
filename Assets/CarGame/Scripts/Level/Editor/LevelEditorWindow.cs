using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : EditorWindow
{
    [MenuItem("MyTools/LevelEditor")]
    private static void Init()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.minSize = new Vector2(300, 600);

        window.Show();
    }


    void SetupLevel()
    {
        // if there is an object named Level
        // then delete it and create a new one
        // named Level with children named
        // Obstacles, Background, EntranceExitPoints
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Setup Level"))
            SetupLevel();

        // Select an obstacle and click add obstacle

        // Add EntraceExit Point Pair
    }
}
