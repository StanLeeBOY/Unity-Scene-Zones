using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

[AddComponentMenu ("Zones/Zone Items/Zone Level Loader")]
public class ZoneLevelLoader : ZoneItem
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float delayBeforeLoad = 0f;
    public UnityEvent beforeLoadEvent;
    public SceneZone.TriggerOption triggerOption;

    public override void Trigger(SceneZone.TriggerOption option)
    {
        if (option == triggerOption)
        {
            StartCoroutine(LoadSceneWithDelay());
        }
    }

    private IEnumerator LoadSceneWithDelay()
    {
        if (beforeLoadEvent != null)
        {
            beforeLoadEvent.Invoke();
        }

        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene(sceneToLoad);
    }
}
