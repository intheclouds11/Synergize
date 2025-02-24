using System;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomMeshModifier : MonoBehaviour
{
    public DestructibleGlobalMeshSpawner destructibleGlobalMeshSpawner;
    private DestructibleMeshComponent _destructibleMeshComponent;
    private bool modStarted;
    // private List<MeshRenderer> renderers = new List<MeshRenderer>();
    private Dictionary<MeshRenderer, Color> dict = new Dictionary<MeshRenderer, Color>();

    private void OnEnable()
    {
        destructibleGlobalMeshSpawner.OnDestructibleMeshCreated.AddListener(OnDestructibleMeshCreated);
    }

    private void OnDisable()
    {
        destructibleGlobalMeshSpawner.OnDestructibleMeshCreated.RemoveListener(OnDestructibleMeshCreated);
    }

    private void Update()
    {
        // On either grip button release
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) ||
            OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) && _destructibleMeshComponent != null)
        {
            BeginModification();
            modStarted = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            modStarted = false;
        }

        if (modStarted && dict.Count > 0)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                var mr = dict.ElementAt(i).Key;
                dict.TryGetValue(mr, out var originalColor);
                var incrementor = 0.1f * i;
                var lerpedColor = Color.Lerp(originalColor, originalColor + Color.white, Mathf.PingPong(Time.time * incrementor, 1));
                mr.material.color = lerpedColor;
            }
        }
    }

    private void OnDestructibleMeshCreated(DestructibleMeshComponent destructibleMeshComponent)
    {
        _destructibleMeshComponent = destructibleMeshComponent;
    }

    private void BeginModification()
    {
        var segments = new List<GameObject>();

        _destructibleMeshComponent.GetDestructibleMeshSegments(segments);
        foreach (var segment in segments)
        {
            // Create a new material with a random color for each segment
            var newMaterial = new Material(Shader.Find("Meta/Lit Transparent"));
            newMaterial.color = Random.ColorHSV();
            newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, 0f);
            if (segment.TryGetComponent<MeshRenderer>(out var renderer))
            {
                renderer.material = newMaterial;
                if (!dict.ContainsKey(renderer))
                {
                    var material = renderer.material;
                    dict.Add(renderer,
                        new Color(material.color.r, material.color.g, material.color.b, material.color.a));
                }
            }
        }

        Debug.Log($"Dictionary count is {dict.Count}!!!!!!");
    }
}