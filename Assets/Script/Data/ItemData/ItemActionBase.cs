using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemActionBase", menuName = "Scriptable Object/Item/ItemActionBase", order = int.MaxValue)]
public class ItemActionBase : ScriptableObject
{
    
    private enum Aciontype
    {
        None,
        State,
        Card
    }
    private enum StateType
    {
        None,
        AttackDamage,
        MaxHP,
        MaxHand,
    }
    private enum CardType
    {
        None,
        Attack,
        Move,
        Gathing
    }
    [SerializeField]
    private Aciontype _Aciontype;
    [SerializeField]
    private StateType _Statetype;
    [SerializeField]
    private CardType _Cardtype;
    [SerializeField]
    private int _Value;

    public int Value { get { return _Value; } }
    
   
    public void Get()
    {
        //액션타입에 따른 실행 방식을 구현하면됩니다.
        Action();

    }
    private void Action()
    {
        switch(_Aciontype)
        {
            case Aciontype.State:
                StateAction();
                break;
            case Aciontype.Card:
                CardAction();
                break;
            default: break;
            
        }
    }

    private void StateAction()
    {
        switch(_Statetype)
        {
            case StateType.AttackDamage:
                GameManager.instance.Player.AttackDamage+= _Value;
                UImanager.Instance.LogTextSet("플레이어 공격력 증가 : " + _Value);
                break;
            case StateType.MaxHP:
                GameManager.instance.Player.MaxHp += _Value;
                GameManager.instance.Player.Hp += _Value;
                UImanager.Instance.LogTextSet("플레이어 최대체력 증가 : " + _Value);
                break;
            case StateType.MaxHand:
                GameManager.instance.Player.PR.MaxCardCount += _Value;
                GameManager.instance.Player.MaxHand += _Value;
                UImanager.Instance.LogTextSet("플레이어 손패개수 증가 : " + _Value);
                break;
            default: break;
        }
    }
    private void CardAction()
    {
        CardDataBase CardData = null;
        switch (_Cardtype)
        {
            case CardType.Attack:
                CardData = Resources.Load<CardDataBase>("DataTable/Card/AttackCard");
                UImanager.Instance.LogTextSet("플레이어 공격카드 개수 증가 :  " + _Value);
                break;
            case CardType.Gathing:
                CardData = Resources.Load<CardDataBase>("DataTable/Card/GathingCard");
                UImanager.Instance.LogTextSet("플레이어 채집카드 개수 증가 :  " + _Value);
                break;
            case CardType.Move:
                CardData = Resources.Load<CardDataBase>("DataTable/Card/MoveCard");
                UImanager.Instance.LogTextSet("플레이어 이동카드 개수 증가 :  " + _Value);
                break;
        }

        if(CardData !=null)
            for(int i =0; i<_Value;i++)
                GameManager.instance.Player.PR.CreateCard(CardData);

    }


}
