using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct RenderToggle
{
    public ScriptableRendererFeature feature;
    public bool isEnabled;
}

public class ToggleRenderFeature : MonoBehaviour
{
      [SerializeField]
    private List<RenderFeatureToggle> renderFeatures = new List<RenderFeatureToggle>();
    [SerializeField]
    private UniversalRenderPipelineAsset pipelineAsset;

    private void Update()
    {
        foreach (RenderFeatureToggle toggleObj in renderFeatures)
        {
            toggleObj.feature.SetActive(toggleObj.isEnabled);
        }
    }
}
