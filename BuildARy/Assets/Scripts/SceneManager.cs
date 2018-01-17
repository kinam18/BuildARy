using System;
using System.Collections;
using UnityEngine;

public static class SceneManager
{
    private static Hashtable sceneArguments;

    internal  static void LoadScene(String sceneName, Hashtable arguments)
    {
        sceneArguments = arguments;
        Application.LoadLevel(sceneName);
    }
    internal static void LoadScene(String sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    internal static Hashtable GetSceneArguments()
    {
        return sceneArguments;
    }


}
