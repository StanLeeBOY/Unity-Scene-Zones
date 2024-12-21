using UnityEngine;
using System.Collections.Generic;
using UltEvents;
using System;

[AddComponentMenu("Zones/Zone Items/Zone Events")]
public class ZoneEvent : ZoneItem
{
    [Serializable]
    public class ZoneEventCallback : UltEvent<GameObject>
    {
    }

    [Space(5)]
    public bool Active = true;
    [Space(2)]
    public ZoneEventCallback OnZoneEnter;
    [Space(2)]
    public ZoneEventCallback OnZoneLeave;
    public List<ZoneEvent> LinkedEvents = new List<ZoneEvent>();

    private ZoneLinks parentZoneLinks;

    private void Start()
    {
        parentZoneLinks = GetComponentInParent<ZoneLinks>();
    }

    public override void Trigger(SceneZone.TriggerOption option)
    {
        if (Active && option == SceneZone.TriggerOption.PRIMARY || option == SceneZone.TriggerOption.SECONDARY)
        {
            switch (option)
            {
                case SceneZone.TriggerOption.PRIMARY:
                    OnPrimaryTrigger();
                    break;
                case SceneZone.TriggerOption.SECONDARY:
                    OnSecondaryTrigger();
                    break;
            }
        }
    }

    private void OnPrimaryTrigger()
    {
        if (parentZoneLinks != null && parentZoneLinks.currentZone == GetComponentInParent<SceneZone>())
        {
            OnZoneEnter.Invoke(this.gameObject);
            UnityEngine.Debug.Log("Primary trigger activated in current zone");
        }
    }

    private void OnSecondaryTrigger()
    {
        OnZoneLeave.Invoke(this.gameObject);
        foreach (var linkedEvent in LinkedEvents)
        {
            linkedEvent.Trigger(SceneZone.TriggerOption.SECONDARY);
        }
        UnityEngine.Debug.Log("Secondary trigger activated for linked zones");
    }
}
