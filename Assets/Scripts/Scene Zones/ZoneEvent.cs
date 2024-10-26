using UnityEngine;
using System.Collections.Generic;
using UltEvents;

[AddComponentMenu ("Zones/Zone Items/Zone Events")]
public class ZoneEvent : ZoneItem
{
    public bool Active = true;
    public bool UseDelayedEvent = false;
    public UltEvent OnZoneEnter;
    public UltEvent OnZoneLeave;
    public List<ZoneEvent> LinkedEvents = new List<ZoneEvent>();
    public SceneZone.TriggerOption triggerType;
    public SceneZone.TriggerOption triggerOption;

    private ZoneLinks parentZoneLinks;

    private void Start()
    {
        parentZoneLinks = GetComponentInParent<ZoneLinks>();
    }

    public override void Trigger(SceneZone.TriggerOption option)
    {
        if (Active && option == triggerOption)
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
            OnZoneEnter.Invoke();
            Debug.Log("Primary trigger activated in current zone");
        }
    }

    private void OnSecondaryTrigger()
    {
        OnZoneLeave.Invoke();
        foreach (var linkedEvent in LinkedEvents)
        {
            linkedEvent.Trigger(SceneZone.TriggerOption.SECONDARY);
        }
        Debug.Log("Secondary trigger activated for linked zones");
    }
}
