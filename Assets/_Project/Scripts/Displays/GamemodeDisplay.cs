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
    [SerializeField] private Image coloredImage;
    [SerializeField] private TMP_Text descriptionText;

    
    public override void Render()
    {
        buttonText.text = item.DisplayName;
        buttonText.color = OppositeColor(item.DisplayColor);
        coloredImage.color = item.DisplayColor;
        descriptionText.text = item.Description;
    }

    private Color OppositeColor(Color color)
    {
        float grayscale = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
        
        return grayscale > 0.5 ? Color.black : Color.white;

    }

    public void OnClick()
    {
        item.Load();
    }
}
