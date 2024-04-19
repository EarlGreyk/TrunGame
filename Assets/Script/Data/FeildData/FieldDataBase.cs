using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FieldDataBase", menuName = "Scriptable Object/Field Data", order = int.MaxValue)]
public class FieldDataBase : ScriptableObject
{


    [SerializeField]
    private Sprite _FieldSprite;

    public Sprite FieldSprite { get { return _FieldSprite; } }

    [SerializeField]
    private int _FieldSize;

    public int FieldSize { get { return _FieldSize; } }

    [SerializeField]
    private int _Difficult;

    public  int Difficult { get { return _Difficult; } }

    [SerializeField]
    private int _MaxStage;

    public int MaxStage { get { return _MaxStage; } }

    [SerializeField]
    private string _FieldName;

    public string FieldName { get { return _FieldName; } }


}

