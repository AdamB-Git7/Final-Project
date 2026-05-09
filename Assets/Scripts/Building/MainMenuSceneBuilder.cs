
// Executes this statement.
#if UNITY_EDITOR

// Imports the UnityEditor namespace.
using UnityEditor;

// Imports the UnityEditor.SceneManagement namespace.
using UnityEditor.SceneManagement;

// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the UnityEngine.UIElements namespace.
using UnityEngine.UIElements;





// Declares the class named MainMenuSceneBuilder.
public static class MainMenuSceneBuilder

// Opens a new code block.
{

    // Applies the MenuItem("Tools/Setup Main Menu Scene") attribute.
    [MenuItem("Tools/Setup Main Menu Scene")]

    // Declares the method named SetupMainMenuScene.
    public static void SetupMainMenuScene()

    // Opens a new code block.
    {

        // Executes this statement.
        const string scenePath = "Assets/Scenes/MainMenu.unity";



        // Declares the variable scene and initializes it.
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);


        // Calls a method.
        Debug.Log("[MainMenuSceneBuilder] Main menu scene is ready. Assign references in the Inspector.");

    // Closes the current code block.
    }

// Closes the current code block.
}

// Executes this statement.
#endif
