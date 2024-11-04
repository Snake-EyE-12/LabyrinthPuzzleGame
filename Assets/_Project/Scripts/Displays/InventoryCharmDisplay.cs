using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCharmDisplay : Display<Item>, Draggable
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text tooltipName;
    [SerializeField] private TMP_Text tooltipDescription;
    [SerializeField] private TMP_Text level;
    [SerializeField] private CanvasGroup canvasGroup;
    
    public override void Render()
    {
        image.sprite = Resources.Load<Sprite>("KeynamedSprites/Items/" + item.name);
        tooltipName.text = item.name;
        tooltipDescription.text = item.description;
    }

    private void EquipTo(Character character)
    {
        character.EquipCharm(item);
        GameManager.Instance.RemoveCharm(item);
        Destroy(this.gameObject);
    }

    private Transform originalParent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(GameManager.Instance.GetCanvasParent());
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        foreach (var hoveredObject in eventData.hovered)
        {
            if (hoveredObject.TryGetComponent(out PostBattleCharacterDisplay pbcd))
            {
                EquipTo(pbcd.GetCharacter());
                pbcd.Render();
                return;
            }
        }
        transform.SetParent(originalParent);
    }
}

public interface Draggable : IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
}