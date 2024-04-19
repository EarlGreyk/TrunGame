using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDesc : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI _ItemName;
    [SerializeField]
    private TextMeshProUGUI _ItemDesc;
    [SerializeField]
    private Image _ItemIcon;

    

 

    public void DescSet(ItemSlot itemSlot)
    {
        _ItemName.text = itemSlot.ItemName;
        _ItemDesc.text = itemSlot.ItemDesc + " + " + itemSlot.ItemValue;
        _ItemIcon.sprite = itemSlot.ItemSprite;
        if(itemSlot.ItemValue != 0)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
   
}
