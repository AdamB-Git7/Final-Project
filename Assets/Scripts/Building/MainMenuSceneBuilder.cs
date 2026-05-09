#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Editor utility that builds and opens the MainMenu scene automatically.
/// </summary>
public static class MainMenuSceneBuilder
{
    [MenuItem("Tools/Setup Main Menu Scene")]
    public static void SetupMainMenuScene()
    {
        const string scenePath = "Assets/Scenes/MainMenu.unity";

        // Open or create the scene
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        Debug.Log("[MainMenuSceneBuilder] Main menu scene is ready. Assign references in the Inspector.");
    }
}
#endif
