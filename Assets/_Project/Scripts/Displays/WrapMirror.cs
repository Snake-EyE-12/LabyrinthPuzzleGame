using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WrapMirror : MonoBehaviour
{
    [SerializeField] private Transform mirror;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform image;

    public void Resize(int gridSize, float spacing)
    {
        //float xPos = (gridSize % 2 == 0) ? 0.5f : 0;
        //Vector3 pos = new Vector3(0, 0.0f, 0);
        //mirror.position = pos;
        //canvas.position = pos;
        image.sizeDelta = new Vector2(gridSize, 1);
    }
}
