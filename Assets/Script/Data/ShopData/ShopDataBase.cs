using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDataBase", menuName = "Scriptable Object/Shop Data",order = int.MaxValue)]
public class ShopDataBase : ScriptableObject
{
    public enum Thing
    {
        CardMove,
        CardAttack,
        CardGathing,
        StatusMaxHp,
        StatusDamage,
        StatusMaxHand,
        Recovery,
        RemoveCard
    }
    [SerializeField]
    private Thing _ThingType;
    public Thing ThingType { get { return _ThingType; } }


    [SerializeField]
    private int _TreePrice;
    public int TreePrice { get { return _TreePrice; } set { _TreePrice = value; } }

    [SerializeField]
    private int _MetalPrice;
    public int MetalPrice { get { return _MetalPrice; } set { _MetalPrice = value; } }
    [SerializeField]
    private int _StonePrice;
    public int StonePrice { get {  return _StonePrice; } set { _StonePrice = value; } }

    [SerializeField]
    private int _TreeValue;
    [SerializeField]
    private int _MetalValue;
    [SerializeField]
    private int _StoneValue;

    public void IncreaseValue()
    {
        _TreePrice += _TreeValue;
        _MetalPrice += _MetalValue;
        _StonePrice += _StoneValue;
    }

    
    

}
