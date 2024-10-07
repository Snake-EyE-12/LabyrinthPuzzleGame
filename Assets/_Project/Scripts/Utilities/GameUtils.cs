using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static int ModPositive(int val, int size) => (val % size + size) % size;
}
