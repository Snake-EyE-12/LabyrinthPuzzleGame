using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeDisplay : Display<Mode>
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    
    public override void Render()
    {
        buttonText.text = item.DisplayName;
        button.onClick.AddListener(() => { OnClick(); });
    }

    public void OnClick()
    {
        item.Load();
    }
}
