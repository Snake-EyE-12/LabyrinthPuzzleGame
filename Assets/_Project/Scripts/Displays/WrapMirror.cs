using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WrapMirror : MonoBehaviour
{
    [SerializeField] private Camera mirror;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform image;

    [SerializeField] private Camera cornerMirror;
    [SerializeField] private RectTransform cornerImage;

    public void Resize(int gridSize, float spacing)
    {
        float diff = (gridSize - 1f) * 0.5f;
        mirror.transform.localPosition = new Vector3(0,-diff * spacing, -10);
        canvas.localPosition = new Vector3(0, (diff + 1) * spacing, 0);
        image.sizeDelta = new Vector2(gridSize * spacing, spacing);
        cornerImage.sizeDelta = new Vector2(spacing, spacing);
        
        cornerMirror.transform.localPosition =  new Vector3(-diff * spacing, -diff * spacing, -10);
        cornerImage.localPosition = new Vector3((diff + 1) * spacing, 0, 0);;

        mirror.orthographicSize = 0.5f * spacing;
        cornerMirror.orthographicSize = 0.5f * spacing;
        mirror.Render();
        cornerMirror.Render();
    }
}
