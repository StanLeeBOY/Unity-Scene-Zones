using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StreamChunk", menuName = "ScriptableObjects/StreamChunk", order = 1)]
public class StreamChunk : ScriptableObject
{
#if UNITY_EDITOR
    public SceneAsset[] scenes;
#endif

    // Method to get scene names
    public string[] GetSceneNames()
    {
#if UNITY_EDITOR
        string[] sceneNames = new string[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            sceneNames[i] = scenes[i].name;
        }
        return sceneNames;
#else
        return new string[0];
#endif
    }
}
