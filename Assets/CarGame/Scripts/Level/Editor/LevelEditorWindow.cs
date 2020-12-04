using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelEditorWindow : EditorWindow
{
    Level m_LevelTemplate;
    ObstacleBase m_ObstacleTemplate;

    [MenuItem("MyTools/LevelEditor")]
    private static void Init()
    {
        var window = GetWindow<LevelEditorWindow>();
        window.minSize = new Vector2(300, 600);

        window.Show();
    }

    Level FindLevelObjectInScene()
    {
        List<GameObject> sceneObjects = new List<GameObject>();

        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(sceneObjects);
        
        for (int i = 0; i < sceneObjects.Count; ++i)
        {
            GameObject go = sceneObjects[i];
            if (go.name == "Level")
            {
                return go.GetComponent<Level>();
            }
        }

        return null;
    }

    void CreateDefaultLevel()
    {
        GameObject level = new GameObject("Level");
        level.transform.position = Vector3.zero;
        Level levelComponent = level.AddComponent<Level>();

        GameObject obstacles = new GameObject("Obstacles");
        obstacles.transform.SetParent(level.transform);

        GameObject background = new GameObject("Background");
        SpriteRenderer backgroundSprite = background.AddComponent<SpriteRenderer>();
        background.transform.SetParent(level.transform);

        GameObject points = new GameObject("EntranceExitPairs");
        points.transform.SetParent(level.transform);

        levelComponent.Init(obstacles.transform, points.transform, backgroundSprite);
    }

    void CreateNewLevel()
    {
        if (m_LevelTemplate != null)
        {
            GameObject level = Instantiate(m_LevelTemplate, null).gameObject;
            level.name = "Level";

            Undo.RecordObject(level, "Level Created");

            EditorApplication.SaveScene();
        }
        else
            CreateDefaultLevel();
    }

    void SetupLevel()
    {
        List<GameObject> sceneObjects = new List<GameObject>();

        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(sceneObjects);
        
        Level level = FindLevelObjectInScene();

        if (level != null)
        {
            if (EditorUtility.DisplayDialog("Reset Level Data?",
                                            "There exists a Level object in the scene. Are you sure to remove it and create a brand new one?\n This action cannot be reversed!",
                                            "Reset", "Cancel"))
            {
                DestroyImmediate(level.gameObject);

                CreateNewLevel();
            }
        }
        else
            CreateNewLevel();
        
    }

    void AddEntranceExitPointPair()
    {
        Level level = FindLevelObjectInScene();
        if(level == null)
        {
            EditorUtility.DisplayDialog("No Level Object",
                                        "The scene does not contain a level object",
                                        "OK");
        }

        level.AddEntranceExitPair();

        Undo.RecordObject(level, "EntranceExitPair added");

        EditorApplication.SaveScene();
    }

    void AddObstacle()
    {
        Level level = FindLevelObjectInScene();
        if (level == null)
        {
            EditorUtility.DisplayDialog("No Level Object",
                                        "The scene does not contain a level object",
                                        "OK");
            return;
        }

        if(m_ObstacleTemplate == null)
        {
            EditorUtility.DisplayDialog("No Obstacle Selected",
                                        "Template obstacle selection is required for spawning",
                                        "OK");
            return;
        }

        level.AddObstacle(m_ObstacleTemplate);
        Undo.RecordObject(level, "Obstacle Created");

        EditorApplication.SaveScene();
    }

    private void OnGUI()
    {
        m_LevelTemplate = EditorGUILayout.ObjectField("Level Template", m_LevelTemplate, typeof(Level), true) as Level;

        if(GUILayout.Button("Setup Level"))
            SetupLevel();

        GUILayout.Space(10);

        m_ObstacleTemplate = EditorGUILayout.ObjectField("Obstacle Template", m_ObstacleTemplate, typeof(ObstacleBase), true) as ObstacleBase;
        if (GUILayout.Button("Create Obstacle"))
            AddObstacle();

        GUILayout.Space(10);

        if (GUILayout.Button("Create EntranceExit Pair"))
            AddEntranceExitPointPair();
    }
}
