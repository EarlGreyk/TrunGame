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
                //�׽�Ʈ��
                count++;
                if (count == 100)
                {
                    Debug.Log("�������ȭ");
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
    /// �÷��̾ �������� ȹ���� ��� �����ۿ� �����Ǿ� �ִ� Acion�� �۵���ŵ�ϴ�.
    /// </summary>
    public void Get()
    {
        //ȹ���� ������ ǥ��
        EffectManager.instance.AoeEffect(transform.position, "Effect/AoE/AoE slash blue");
        SoundManager.instance.AudioPlay("Sound/Effect/Item/Item_Get");
        //������ �׼� ����
        if(_Acion != null) 
            _Acion.Get();
        //������ ȹ��� �ش� �������� �⺻ �������� �κ��丮�� ������ ��.
        ItemDataBase item = Resources.Load<ItemDataBase>("DataTable/Item/ItemNumber/Item_" + _ItemID);
        Debug.Log(item);
        Debug.Log(InventoryManager.Instance);
        InventoryManager.Instance.InventorySet(item);

        UnitStateUiMnager.Instance.StateGet(GameManager.instance.Player);



    }

}
