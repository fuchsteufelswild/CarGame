using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level m_Target;

    private void OnEnable()
    {
        m_Target = target as Level;
    }

    Vector3 GetScreenLocation(Vector3 viewport)
    {
        return new Vector3(Screen.width * viewport.x, Screen.height * viewport.y, 0f);
    }

    private void OnSceneGUI()
    {
        float startY = 0.03f;
        float startX = 0.01f;
        float increment = 0.06f;

        bool changeMade = false;

        Handles.BeginGUI();
        GUI.Box(new Rect(0, 0, 300f, Screen.height / 4f), "Level Editor");

        Vector3 screenLocation = GetScreenLocation(new Vector3(startX, startY));

        if(GUI.Button(new Rect(screenLocation.x, screenLocation.y, 200f, Screen.height / 15f), "Add Static Obstacle"))
        {
            m_Target.AddStaticObstacle();

            Undo.RecordObject(m_Target, "Static Obstacle Added");
            changeMade = true;
        }

        startY += increment;
        screenLocation = GetScreenLocation(new Vector3(startX, startY));

        if (GUI.Button(new Rect(screenLocation.x, screenLocation.y, 200f, Screen.height / 15f), "Add Moving Obstacle"))
        {
            m_Target.AddMovingObstacle();

            Undo.RecordObject(m_Target, "Moving Obstacle Added");
            changeMade = true;
        }

        startY += increment;
        screenLocation = GetScreenLocation(new Vector3(startX, startY));

        if (GUI.Button(new Rect(screenLocation.x, screenLocation.y, 200f, Screen.height / 15f), "Add Entrance/Exit Pair"))
        {
            m_Target.AddEntranceExitPair();

            Undo.RecordObject(m_Target, "Entrance/Exit Added");
            changeMade = true;
        }
        Handles.EndGUI();

        if (changeMade)
            EditorApplication.SaveScene();
    }
}
