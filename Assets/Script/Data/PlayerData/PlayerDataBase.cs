using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "Scriptable Object/Player Data", order = int.MaxValue)]
public class PlayerDataBase : ScriptableObject
{

    [SerializeField]
    private string _Name;
    public string Name { get { return _Name; } }


    [SerializeField]
    private int _MaxHp;
    public int MaxHp { get { return _MaxHp; } }

    [SerializeField]
    private int _AttackDamage;
    public int AttackDamage { get { return _AttackDamage; } }

    [SerializeField]
    private int _AttackRange;
    public int AttackRange { get { return _AttackRange; } }

    [SerializeField]
    private int _MaxHand;
    public int MaxHand { get { return _MaxHand; } }


    [SerializeField]
    private int _Tree;
    public int Tree { get { return _Tree; } }

    [SerializeField]
    private int _Stone;

    public int Stone { get { return _Stone; } }

    [SerializeField]
    private int _Metal;
    public int Metal { get { return _Metal; } }

    [SerializeField]
    private int _MoveCard;
    public int MoveCard { get { return _MoveCard; } }
    [SerializeField]
    private int _GathingCard;
    public int GathingCard { get {  return _GathingCard; } }    
    [SerializeField]
    private int _AttackCard;
    public int AttackCard { get { return _AttackCard; } }


    [SerializeField]
    private Sprite _PlayerIcon;
    public Sprite PlayerIcon { get { return _PlayerIcon; } }



}
