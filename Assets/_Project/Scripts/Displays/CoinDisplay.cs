using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinDisplay : Display<int>
{
    [SerializeField] private TMP_Text textbox;
    public override void Render()
    {
        textbox.text = "" + item;
    }
}
