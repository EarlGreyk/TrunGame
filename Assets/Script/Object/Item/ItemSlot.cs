using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private string _ItemName;
    public string ItemName { get { return _ItemName; } }
    private string _ItemDesc;
    public string ItemDesc { get { return _ItemDesc; } }

    [SerializeField]
    private Image _ItemImage;
    
    private Sprite _ItemSprite;
    public Sprite ItemSprite { get { return _ItemSprite; } }
    private int _ItemValue;
    public int ItemValue { get { return _ItemValue; } }
    [SerializeField]
    private TextMeshProUGUI _ItemValueText;
    public void ItemSlotSet(ItemDataBase itemData)
    {
        _ItemName = itemData.ItemName;
        _ItemDesc = itemData.ItemDesc;
        _ItemSprite = itemData.ItemSprite;
        _ItemValue = itemData.Acion.Value;
        _ItemValueText.text = _ItemValue.ToString();
        _ItemImage.sprite = _ItemSprite;
    }

    public void ItemSlotSet(InventoryItemSaveData itemData)
    {
        _ItemName = itemData.itemName;
        _ItemDesc = itemData.itemDesc;
        _ItemSprite = Resources.Load<Sprite>("UI/Card/Sprite/"+itemData.itemSpritePath);
        _ItemValue = itemData.itemValue;
        _ItemValueText.text = _ItemValue.ToString();
        _ItemImage.sprite = _ItemSprite;
    }






}
