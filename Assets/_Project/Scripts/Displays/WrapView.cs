using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

public class WrapView : MonoBehaviour
{
    [SerializeField] private List<WrapMirror> wrapMirrors;
    [SerializeField] private List<RenderTexture> renderTextures;
    [SerializeField] private int gridSize;
    [SerializeField] private float spacing;
    [SerializeField] private int squareResolution;


    [Button]
    private void ChangeGridSizeManually()
    {
        ChangeGridSize(gridSize, spacing, squareResolution);
    }
    private void Resize(RenderTexture renderTexture, int width, int height) {
        if (renderTexture) {
            renderTexture.Release();
            renderTexture.width = width;
            renderTexture.height = height; 
        }
    }

    private void Awake()
    {
        ChangeGridSize(DataHolder.currentMode.GridSize, VisualDataHolder.Instance.spacing, squareResolution);
    }
    private void ChangeGridSize(int size, float space, int resolution) {
        foreach (var rt in renderTextures)
        {
            Resize(rt, (int)(size * resolution * space), (int)(resolution * space));
        }
        float squarePos = (size - 1f) * 0.5f * space;
        transform.position = new Vector3(squarePos, squarePos, 0);
        foreach (var wm in wrapMirrors)
        {
            wm.Resize(size, space);
        }
    }
}
