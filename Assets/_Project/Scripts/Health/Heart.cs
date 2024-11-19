using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Image heartImage;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Image fire;
    [SerializeField] private Image bleed;
    
    public void Fill(HeartState state)
    {
        heartImage.sprite = fullHeart;
        fire.enabled = state.burning;
        bleed.enabled = state.bleeding;
        if (state.poisoned)
        {
            heartImage.color = DataHolder.keywordColorEquivalenceTable.GetColor("Poison");
        }
        else heartImage.color = state.mainColor;
    }

    public void Empty()
    {
        heartImage.sprite = emptyHeart;
        fire.enabled = false;
        bleed.enabled = false;
        heartImage.color = Color.white;

    }
}

public class HeartState
{
    public Color mainColor;
    public HeartState(bool poisoned, bool bleeding, bool burning, Color okColor)
    {
        this.mainColor = okColor;
        this.poisoned = poisoned;
        this.bleeding = bleeding;
        this.burning = burning;
    }
    public bool poisoned;
    public bool bleeding;
    public bool burning;
    public bool healthy => !poisoned && !bleeding && !burning;
}