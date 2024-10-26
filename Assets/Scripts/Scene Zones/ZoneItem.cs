using UnityEngine;


public abstract class ZoneItem : MonoBehaviour
{
    public Color gizmoColor = Color.yellow;
    public abstract void Trigger(SceneZone.TriggerOption option);
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        OnDrawZoneItemGizmos(false);
    }
    private void OnDrawGizmosSelected()
    {
        OnDrawZoneItemGizmos(true);
    }

    private void OnDrawZoneItemGizmos(bool selected)
    {
        float alpha = selected ? 0.5f : 0.2f;
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, alpha);

        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        style.alignment = TextAnchor.MiddleCenter;
    }
    #endif
}

