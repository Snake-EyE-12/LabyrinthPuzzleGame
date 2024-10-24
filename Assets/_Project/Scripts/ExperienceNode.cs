using UnityEngine;
using UnityEngine.UI;

public class ExperienceNode : MonoBehaviour
{
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private Image xpPointImage;
    public void IsDisplayed(bool visible)
    {
        xpPointImage.color = visible ? onColor : offColor;
    }
}