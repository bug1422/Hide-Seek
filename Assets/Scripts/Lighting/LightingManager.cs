using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField]
    private Light DirectionalLight;
    [SerializeField]
    private LightingPreset Preset;
    [SerializeField, Range(0, 24)]
    private float TimeOfDay;

    public bool autoUpdate = false;
    public float time;

    public bool turnOff = false;
    private void Update()
    {
        if (Preset == null) return;
        if (Application.isPlaying)
        {
            UpdateLighting();
        }
        else
        {
            UpdateLighting();
        }
        time = TimeOfDay;
    }
    public void UpdateLighting()
    {
        if (turnOff)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            DirectionalLight.Reset();
        }
        else
        {
            UpdateLighting(TimeOfDay / 24);
            time = TimeOfDay;
        }
    }
    private void UpdateLighting(float timePercent)
    {
        if(RenderSettings.ambientMode != UnityEngine.Rendering.AmbientMode.Flat) RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
    
    private void OnValidate()
    {
        if (DirectionalLight != null) return;
        if (RenderSettings.sun != null) DirectionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
        if (Preset != null)
        {
            Preset.OnValuesUpdated -= UpdateLighting;
            Preset.OnValuesUpdated += UpdateLighting;
        }
        
    }
}
