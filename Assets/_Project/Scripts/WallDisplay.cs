using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisplay : MonoBehaviour
{
    public void SetVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
