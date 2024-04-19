using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string _ItemName;
    public string ItemName { get { return _ItemName; } set { _ItemName = value; } }
    private int _ItemID;
    public int ItemID { get { return _ItemID; }set { _ItemID = value; } }
    private ItemActionBase _Acion;
    public ItemActionBase Acion { set { _Acion = value; } }

    private string _ItemDesc;
    public string ItemDesc { get { return _ItemDesc; } set { _ItemDesc = value;} }

    private void Start()
    {
        if (_ItemID == 0)
        {
            ItemDataBase item = null;
            int number = 0;
            int count = 0;
            while (item == null)
            {
                number = Random.Range(1, 10);
                item = Resources.Load<ItemDataBase>("DataTable/Item/ItemNumber/Item_" + number);
                //테스트용
                count++;
                if (count == 100)
                {
                    Debug.Log("연산과부화");
                    break;
                }

            }
            if (item != null)
            {
                Set(item);
            }
               
        }
    }

    public void Set(ItemDataBase itemdata)
    {
        _ItemName = itemdata.name;
        _ItemID = itemdata.ItemID;
        _Acion = itemdata.Acion;
        _ItemDesc = itemdata.ItemDesc;

    }





    /// <summary>
    /// 플레이어가 아이템을 획득할 경우 아이템에 부착되어 있는 Acion을 작동시킵니다.
    /// </summary>
    public void Get()
    {
        //획득한 아이템 표시
        EffectManager.instance.AoeEffect(transform.position, "Effect/AoE/AoE slash blue");
        SoundManager.instance.AudioPlay("Sound/Effect/Item/Item_Get");
        //아이템 액션 시작
        if(_Acion != null) 
            _Acion.Get();
        //아이템 획득시 해당 아이템의 기본 정보값을 인벤토리에 넣으면 끝.
        ItemDataBase item = Resources.Load<ItemDataBase>("DataTable/Item/ItemNumber/Item_" + _ItemID);
        Debug.Log(item);
        Debug.Log(InventoryManager.Instance);
        InventoryManager.Instance.InventorySet(item);

        UnitStateUiMnager.Instance.StateGet(GameManager.instance.Player);



    }

}
