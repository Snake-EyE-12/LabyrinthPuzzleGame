using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionDisplay : MonoBehaviour
{
    public SelectableGroupType type;
    public abstract void StartSelection();
    public abstract void EndSelection();
}
