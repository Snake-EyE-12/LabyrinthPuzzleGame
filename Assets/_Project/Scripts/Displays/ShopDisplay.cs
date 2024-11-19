using System.Collections.Generic;
using Guymon.DesignPatterns;
using TMPro;
using UnityEngine;

public class ShopDisplay : Display<ShopData>
{
    [SerializeField] private Destinator destinator;
    [SerializeField] private Vector3 offScreenPosition;
    [SerializeField] private Vector3 onScreenPosition;

    [SerializeField] private Transform productTable;
    [SerializeField] private ProductDisplay productDisplayPrefab;

    [SerializeField] private TMP_Text coinAmountTextbox;
    
    private void OnEnable()
    {
        EventHandler.AddListener("Coins/Change", SetCoinValue);
        EventHandler.AddListener("Round/FightOver", RemoveShop);
    }

    private void RemoveShop(EventArgs args)
    {
        Destroy(this.gameObject);
        EventHandler.RemoveListenerLate("Round/FightOver", RemoveShop);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener("Coins/Change", SetCoinValue);
    }

    private void SetCoinValue(EventArgs args)
    {
        coinAmountTextbox.text = (args as IntEventArgs).value + "";
    }
    public override void Render()
    {
        FillShop();
        Open();
    }

    private void FillShop()
    {
        foreach (var product in item.products)
        {
            Instantiate(productDisplayPrefab, productTable).Set(product);
        }
    }

    public void Open()
    {
        destinator.MoveTo(onScreenPosition, true);
        coinAmountTextbox.text = GameManager.Instance.CoinCount + "";
    }

    public void Close()
    {
        destinator.MoveTo(offScreenPosition, true);
    }
}

public class ShopData
{
    public List<Item> products = new();

    public ShopData(int round)
    {
        for (int i = 0; i < DataHolder.currentMode.ProductsPerShop; i++)
        {
            products.Add(new Item(Item.Load(round)));
        }
    }
}
