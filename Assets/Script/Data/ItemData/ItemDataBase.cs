using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Object/Item/ItemDataBase", order = int.MaxValue)]
public class ItemDataBase : ScriptableObject
{

    //아이템 공통 속성
    [SerializeField]
    private string _ItemName;
    [SerializeField]
    private int _ItemID;

    [SerializeField]
    private ItemActionBase _Acion;

    [SerializeField]
    private string _ItemDesc;
    [SerializeField]
    private Sprite _ItemSprite;



    public string ItemName { get { return _ItemName; } }
    public int ItemID { get { return _ItemID; } }

    public ItemActionBase Acion { get {  return _Acion; } }

    public string ItemDesc { get { return _ItemDesc; } }

    public Sprite ItemSprite { get { return _ItemSprite; } }


}
