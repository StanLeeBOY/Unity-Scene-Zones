using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ZoneItem : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;

    public abstract void Trigger(SceneZone.TriggerOption option);

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(0.25f, 0.25f, 0.25f));
        // Dynamically retrieve the name of the GameObject and component type
        string componentName = this.GetType().Name; // e.g., "ZoneAmbience"
        string objectName = gameObject.name; // e.g., "ZoneA"

        Handles.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        Handles.Label(
            transform.position + Vector3.up * 0.5f, 
            $"{componentName} of {objectName}", 
            GetLabelStyle()
        );
    }

    private void OnDrawGizmosSelected()
    {
        OnDrawZoneItemGizmos(true);
    }

    private void OnDrawZoneItemGizmos(bool selected)
    {
        float alpha = selected ? 0.5f : 0.2f;
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, alpha);
    }

    private GUIStyle GetLabelStyle()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
        style.fontSize = 12;
        style.alignment = TextAnchor.MiddleCenter;
        return style;
    }
#endif
}
