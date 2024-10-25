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
    private void ChangeGridSize()
    {
        foreach (var rt in renderTextures)
        {
            Resize(rt, gridSize * squareResolution, squareResolution);
        }

        foreach (var wm in wrapMirrors)
        {
            wm.Resize(gridSize, spacing);
        }
    }
    private void Resize(RenderTexture renderTexture, int width, int height) {
        if (renderTexture) {
            renderTexture.Release();
            renderTexture.width = width;
            renderTexture.height = height; 
        }
    }
}
