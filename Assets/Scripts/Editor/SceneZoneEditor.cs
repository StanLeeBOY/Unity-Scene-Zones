using UnityEngine;
using UnityEditor;

public class SceneZoneEditor
{
    // Add a "Scene Zone" entry in the GameObject menu (appears on right-click in Hierarchy)
    [MenuItem("GameObject/SDK/Zones/SceneZone", false, 10)]
    static void CreateSceneZone(UnityEditor.MenuCommand menuCommand) // Explicit namespace
    {
        // Create a new GameObject for the Scene Zone
        GameObject sceneZone = new GameObject("Scene Zone");

        // Add a BoxCollider and configure it as a trigger
        BoxCollider collider = sceneZone.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        // Add other relevant components
        sceneZone.AddComponent<SceneZone>();
        sceneZone.AddComponent<ZoneLinks>();

        // Register the creation in the Undo system (to allow Undo in the editor)
        Undo.RegisterCreatedObjectUndo(sceneZone, "Create Scene Zone");

        // Select the newly created object in the Hierarchy
        Selection.activeObject = sceneZone;
    }
}
