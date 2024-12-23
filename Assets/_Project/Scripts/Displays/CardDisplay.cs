using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : Display<Card>, Selectable
{
    [SerializeField] private Image colorRingImageRenderer;
    [SerializeField] private Image iconImageRenderer;
    [SerializeField] private Transform valuePointsParent;
    [SerializeField] private GameObject valuePointPrefab;
    [SerializeField] private TMP_Text descriptionTextbox;
    [SerializeField] private TMP_Text valueTextbox;
    
    
    
    [Header("Non-Display")]
    [SerializeField] private SelectionDisplay selectionIndicator;
    [SerializeField] private TileOnCardDisplay tileForDisplay;
    private void Update()
    {
        if (!selected || GameManager.Instance.GetSelectionType() != SelectableGroupType.Tile) return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            Deselect();
            selected = false;
            GameManager.Instance.SetCardToPlace(null);
        }
    }
    
    public override void Render()
    {
        Color cardColor = DataHolder.characterColorEquivalenceTable.GetColor(item.owner);
        colorRingImageRenderer.color = cardColor;
        iconImageRenderer.color = cardColor;
        iconImageRenderer.sprite = Resources.Load<Sprite>("KeynamedSprites/CharacterIcons/" + item.owner);
        valueTextbox.text = (item.GetTile().ability != null) ? item.GetTile().ability.GetValue() + "" : "";
        Ability a = item.GetTile().ability;
        if (a != null)
        {
            descriptionTextbox.gameObject.SetActive(true);
            var descriptionBuilder = "";
            foreach (var keyword in a.keys)
            {
                descriptionBuilder += keyword.GetKeywordName().ConvertToString() + ", ";
            }
            descriptionTextbox.text = descriptionBuilder.Substring(0, descriptionBuilder.Length - 2);
            
            valuePointsParent.gameObject.SetActive(a.GetValue() > 0);
            foreach (Transform child in valuePointsParent)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < a.GetValue(); i++)
            {
                var valuePoint = Instantiate(valuePointPrefab, valuePointsParent);
            }
        }
        else
        {
            valuePointsParent.gameObject.SetActive(false);
            descriptionTextbox.gameObject.SetActive(false);

        }
        
        tileForDisplay.Set(item.GetTile());
    }

    public void SetAsActiveSelection(bool active)
    {
        selectionIndicator.Activated(active);
    }

    public void SelectViaClick()
    {
        if(IsCurrentlySelectable()) Activate(null);
    }

    private bool selected;

    public void RotateTile(RotationDirection direction, int amount)
    {
        item.GetTile().rotation.Rotate(direction, amount);
        Render();
    }

    public Card GetCard()
    {
        return item;
    }

    public Tile GetTile()
    {
        return item.GetTile();
    }

    public void Select()
    {
        if (!IsCurrentlySelectable()) return;
        selectionIndicator.StartSelection();
    }

    public void Deselect()
    {
        selectionIndicator.EndSelection();
    }

    public int GetOrderValue()
    {
        return 0;
    }

    public bool IsCurrentlySelectable()
    {
        return GameManager.Instance.GetSelectionType() == SelectableGroupType.Card;
    }

    public void Activate(SelectableActivatorData data)
    {
        AudioManager.Instance.Play("ButtonClick");
        GameManager.Instance.SetCardToPlace(this);
        tileForDisplay.TryShowRotateTutorial();
    }
    private void Start()
    {
        GameManager.Instance.AddSelectable(this, selectionIndicator.type);
    }

    [SerializeField] private Destinator destinator;
    [SerializeField] private float xDistanceToOffScreen;
    public void DiscardFromPlay()
    {
        if (transform == null) return;
        MoveVisually(transform.position + Vector3.left * xDistanceToOffScreen, 1);
        RemoveCard(destinator.GetBaseTime());
    }

    public void RemoveCard(float time = 0)
    {
        GameManager.Instance.RemoveSelectable(this, selectionIndicator.type);
        Destroy(this.gameObject, time);
    }
    
    public void MoveVisually(Vector3 position, float speedModifier)
    {
        destinator.MoveTo(new DestinationData(position, destinator.GetBaseTime() * speedModifier, false));
    }

}

public interface Pointer : IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    
}