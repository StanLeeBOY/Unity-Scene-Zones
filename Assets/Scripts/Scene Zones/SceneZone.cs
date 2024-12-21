using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[AddComponentMenu("Zones/Scene Zone")]
public class SceneZone : MonoBehaviour
{
    public enum TriggerOption { PRIMARY, SECONDARY, PRIMARY_AND_SECONDARY }

    public LayerMask triggerLayers; // Layers for triggering
    private List<ZoneItem> zoneItems;
    private ZoneLinks zoneLinks;
    public Color gizmoColor = Color.cyan;
    private HashSet<Transform> triggeredParents = new HashSet<Transform>(); // To track triggered parents

    private void Awake()
    {
        zoneItems = GetComponents<ZoneItem>().ToList();
        zoneLinks = GetComponent<ZoneLinks>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTriggerObject(other))
        {
            TriggerZone(TriggerOption.PRIMARY);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsTriggerObject(other))
        {
            TriggerZone(TriggerOption.SECONDARY);
        }
    }

    private bool IsTriggerObject(Collider other)
    {
        // Check if the layer is within the trigger layers
        if ((triggerLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return false;
        }

        // Check if the object has been triggered before
        Transform current = other.transform;
        while (current != null)
        {
            if (triggeredParents.Contains(current))
            {
                return false; // Already triggered
            }
            triggeredParents.Add(current);
            return true;
            current = current.parent;
        }

        return false;
    }

    public void TriggerZone(TriggerOption option)
    {
        foreach (var zoneItem in zoneItems)
        {
            zoneItem.Trigger(option); // Trigger all zone items
        }

        if (zoneLinks != null)
        {
            zoneLinks.TriggerConnectedZones(option); // Trigger linked zones
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawZoneGizmo(false);
    }

    private void OnDrawGizmosSelected()
    {
        DrawZoneGizmo(true);
    }

    private void DrawZoneGizmo(bool selected)
    {
        Collider zoneCollider = GetComponent<Collider>();
        if (zoneCollider == null) return;

        float alpha = selected ? 0.5f : 0.2f;
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, alpha);

        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        style.alignment = TextAnchor.MiddleCenter;

        if (zoneCollider is BoxCollider boxCollider)
        {
            if (selected)
            {
                Gizmos.DrawCube(transform.position, boxCollider.size);
            }
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
            Gizmos.DrawWireCube(transform.position, boxCollider.size);
            Handles.Label(transform.position, gameObject.name, style);
        }
        else if (zoneCollider is SphereCollider sphereCollider)
        {
            if (selected)
            {
                Gizmos.DrawSphere(sphereCollider.bounds.center, sphereCollider.radius);
            }
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
            Gizmos.DrawWireSphere(sphereCollider.bounds.center, sphereCollider.radius);
            Handles.Label(sphereCollider.bounds.center, gameObject.name, style);
        }
    }
#endif
}
