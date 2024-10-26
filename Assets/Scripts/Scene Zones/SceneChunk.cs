using UnityEngine;
using UnityEngine.SceneManagement;
using UltEvents;
using System.Collections;
using System.Collections.Generic;
[AddComponentMenu ("Zones/Zone Items/Scene Chunk")]
public class SceneChunk : ZoneItem
{
    public StreamChunk streamChunk;
    public UltEvent OnChunkLoaded;
    public UltEvent OnChunkUnloaded;
    
    private static HashSet<string> globallyLoadedScenes = new HashSet<string>();
    private List<string> localLoadedScenes = new List<string>();
    private Dictionary<Rigidbody, bool> originalKinematicStates = new Dictionary<Rigidbody, bool>();

    public override void Trigger(SceneZone.TriggerOption option)
    {
        if (option == SceneZone.TriggerOption.PRIMARY)
        {
            StartCoroutine(LoadScenes());
        }
        else if (option == SceneZone.TriggerOption.SECONDARY)
        {
            StartCoroutine(UnloadScenes());
        }
    }

    private IEnumerator LoadScenes()
    {
        string[] sceneNames = streamChunk.GetSceneNames();
        foreach (string sceneName in sceneNames)
        {
            if (!globallyLoadedScenes.Contains(sceneName))
            {
                globallyLoadedScenes.Add(sceneName);
                localLoadedScenes.Add(sceneName);
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = false;

                // Wait until the scene is almost fully loaded
                while (asyncLoad.progress < 0.9f)
                {
                    yield return null;
                }
                
                asyncLoad.allowSceneActivation = true;

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                // Set the kinematic states for Rigidbody objects in the loaded scene
                SetRigidbodiesKinematic(sceneName, false);
                OnChunkLoaded.Invoke(); // Invoke the event after loading
                Debug.Log("Loaded Scene: " + sceneName);
            }
        }
    }

    private IEnumerator UnloadScenes()
    {
        foreach (string sceneName in localLoadedScenes)
        {
            if (globallyLoadedScenes.Contains(sceneName))
            {
                // Set all rigidbodies to kinematic before unloading
                SetRigidbodiesKinematic(sceneName, true);

                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

                // Wait until the scene is fully unloaded
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }

                globallyLoadedScenes.Remove(sceneName);
                OnChunkUnloaded.Invoke(); // Invoke the event after unloading
                Debug.Log("Unloaded Scene: " + sceneName);
            }
        }
        localLoadedScenes.Clear();
    }

    private void SetRigidbodiesKinematic(string sceneName, bool kinematic)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>(true);
                foreach (Rigidbody rb in rigidbodies)
                {
                    if (kinematic)
                    {
                        if (!originalKinematicStates.ContainsKey(rb))
                        {
                            originalKinematicStates[rb] = rb.isKinematic;
                        }
                        rb.isKinematic = true; // Set to kinematic when unloading
                    }
                    else
                    {
                        if (originalKinematicStates.TryGetValue(rb, out bool originalState))
                        {
                            rb.isKinematic = originalState; // Restore original state
                            originalKinematicStates.Remove(rb); // Clean up the dictionary entry
                        }
                    }
                }
            }
        }
    }
}
