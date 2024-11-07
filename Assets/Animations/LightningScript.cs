using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    [SerializeField]
    float intensity = 3f;
    [SerializeField]
    List<GameObject> lights = new List<GameObject>();
    [SerializeField]
    float duration = 1.5f;
    float timeDelay;
    bool isStriking = false;
    float addIntensity;
    float maxIntensity;
    private void Start()
    {
        maxIntensity = RenderSettings.ambientIntensity + intensity;
        addIntensity = intensity / duration;
    }
    private void Update()
    {
        if (!isStriking)
        {
            isStriking = true;
            StartCoroutine(StartLightning(Random.Range(5f, 10f)));
        }
    }
    
    public void RaiseAmbientIntensity()
    {
        while (true)
        {
            RenderSettings.ambientIntensity += addIntensity;
            if (RenderSettings.ambientIntensity >= maxIntensity) break;
        }
        StartCoroutine(LightningDuration(Random.Range(0.2f, 1f)));
        Debug.Log("start");
    }
    private IEnumerator StartLightning(float duration)
    {
        yield return new WaitForSeconds(duration);
        RaiseAmbientIntensity();
    }

    private IEnumerator LightningDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("end");
        RenderSettings.ambientIntensity -= intensity;
        foreach(var light in lights)
        {
            var component = light.GetComponent<Light>();
            StartCoroutine(Flickering(component));
        }
    }

    private IEnumerator Flickering(Light light)
    {
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        light.enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        light.enabled = true;
        timeDelay = Random.Range(0.01f, 0.1f);
        yield return new WaitForSeconds(timeDelay);
        light.enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        light.enabled = true;
        isStriking = false;
    }
}
