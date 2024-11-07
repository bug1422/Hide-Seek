using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(LightingManager))]
public class LightingManagerExtended : Editor
{
    public override void OnInspectorGUI()
    {
        LightingManager lightingManager = (LightingManager)target;
        if (DrawDefaultInspector())
        {
            if (lightingManager.autoUpdate)
            {
                lightingManager.UpdateLighting();
            }
        }
        if (GUILayout.Button("Update"))
        {
            lightingManager.UpdateLighting();
        }
    }
}
#endif