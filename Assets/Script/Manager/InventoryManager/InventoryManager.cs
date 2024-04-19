using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField]
    private List<ItemSlot> _Slots;

    public List<ItemSlot> Slots { get { return _Slots; } }

    [SerializeField]
    private ItemDesc _SlotDesc;

    public ItemDesc SlotDesc {get { return _SlotDesc; }}


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
            
    }
    private void Start()
    {
        if(SaveLoadManager.instance.IsLoad)
        {
            InventoryLoad(SaveLoadManager.instance.InventorySaveData);
        }
    }

    public void InventorySet(ItemDataBase itemData)
    {
        for (int i = 0; i < _Slots.Count; i++)
        {
            if (_Slots[i].ItemName == null)
            {
                _Slots[i].ItemSlotSet(itemData);
                break;
            }
        }
    }
    private void InventoryLoad(InventoryManagerSaveData invenSaveData)
    {
        for (int i = 0; i < invenSaveData.InvenItemDataList.Count; i++)
        {
            _Slots[i].ItemSlotSet(invenSaveData.InvenItemDataList[i]);
        }
    }

}
