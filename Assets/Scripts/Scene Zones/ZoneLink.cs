using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu ("Zones/Zone Link")]
public class ZoneLinks : MonoBehaviour
{
    public List<SceneZone> connectedZones = new List<SceneZone>();
    public SceneZone currentZone { get; private set; }

    private void Awake()
    {
        currentZone = GetComponent<SceneZone>();
        if (currentZone == null)
        {
            Debug.LogError("ZoneLinks script must be attached to a GameObject with a SceneZone component.");
        }
    }


    public void TriggerConnectedZones(SceneZone.TriggerOption option)
    {
        foreach (var zone in connectedZones)
        {
            zone.TriggerZone(option);
        }
    }

    public bool IsZoneConnected(SceneZone zone)
    {
        return connectedZones.Contains(zone);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.cyan;
        foreach (var zone in connectedZones)
        {
            if (zone != null)
            {
                    Gizmos.DrawLine(transform.position, zone.transform.position);
            }
        }
    }
}
