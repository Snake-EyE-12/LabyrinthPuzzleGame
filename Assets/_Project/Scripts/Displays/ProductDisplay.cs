using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductDisplay : Display<Item>
{
    [SerializeField] private TMP_Text priceBox;
    [SerializeField] private Image productImage;
    [SerializeField] private TMP_Text toolTipName;
    [SerializeField] private TMP_Text tooltipNumber;
    [SerializeField] private TMP_Text tooltipDescription;
    public override void Render()
    {
        priceBox.text = "$" + item.price;
        productImage.sprite = Resources.Load<Sprite>("KeynamedSprites/Items/" + item.name);
        toolTipName.text = item.name;
        tooltipNumber.text = item.degree + "";
        tooltipDescription.text = item.description;
    }

    public void Buy()
    {
        if (GameManager.Instance.CoinCount >= item.price)
        {
            DataHolder.coinsSpent += item.price;
            GameManager.Instance.CoinCount -= item.price;
            GameManager.Instance.GainCharm(item);
            AudioManager.Instance.Play("Purchase");
            Destroy(this.gameObject);
        }
    }
}

