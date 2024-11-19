using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void Awake()
    {
        transform.position = GameManager.Instance.AbilityUser.GetTransform().position;
    }
}
