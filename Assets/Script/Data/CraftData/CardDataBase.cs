using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataBase", menuName = "Scriptable Object/Card Data", order = int.MaxValue)]
public class CardDataBase : ScriptableObject
{
    /// <summary>
    /// 데이터값.
    ///데이터 번호, 분류값, 이름 , value, 설명
    ///
    /// </summary>
    ///
    public enum ValueType
    {
        None,
        HP,
        Move,
        Attack,
        Gathing,
        
    }
    [SerializeField]
    private ValueType _DataType;    
    public ValueType DataType { get { return _DataType; } }

    [SerializeField]
    private int _Index;
    public int Index { get { return _Index; } }
    [SerializeField]
    private string _SpritePath;
    public string SpritePath { get { return _SpritePath; }
    }
    [SerializeField]
    private string _Name;
    public string Name { get { return _Name; } }

    [SerializeField]
    private int _Value;
    public int Value { get { return _Value; } }

    [SerializeField]
    private string _Desc;
    public string Desc { get { return _Desc;} }


    public void CreateDataBase(CardDataSaveData CardSaveData)
    {
        _DataType = CardSaveData.cardDataType;
        _SpritePath = CardSaveData.cardspritepath;
        _Name = CardSaveData.cardname;
        _Value = CardSaveData.cardvalue;
        _Desc = CardSaveData.desc;
    }

}
