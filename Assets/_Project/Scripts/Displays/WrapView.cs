using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

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
        
    }
}
