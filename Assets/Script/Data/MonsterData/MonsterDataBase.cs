using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataBase : ScriptableObject
{

    //공통분모
    [SerializeField]
    private string _Name;
    [SerializeField]
    private int _MaxHp;
    [SerializeField]
    private int _MaxActionCount;
    [SerializeField]
    private float _AttackRange;
    [SerializeField]
    private float _TargetRange;
    [SerializeField]
    private float _AttackDamage;
    
    public string Name { get { return _Name; } }
    public int MaxHp { get { return _MaxHp; } }
    public int MaxActionCount { get { return _MaxActionCount; } }
    public float AttackRange { get { return _AttackRange; } }
    public float TargetRange { get { return _TargetRange; } }
    public float AttackDamage { get { return _AttackDamage; } }


}
