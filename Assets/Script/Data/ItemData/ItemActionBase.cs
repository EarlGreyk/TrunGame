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
        //�׼�Ÿ�Կ� ���� ���� ����� �����ϸ�˴ϴ�.
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
                UImanager.Instance.LogTextSet("�÷��̾� ���ݷ� ���� : " + _Value);
                break;
            case StateType.MaxHP:
                GameManager.instance.Player.MaxHp += _Value;
                GameManager.instance.Player.Hp += _Value;
                UImanager.Instance.LogTextSet("�÷��̾� �ִ�ü�� ���� : " + _Value);
                break;
            case StateType.MaxHand:
                GameManager.instance.Player.PR.MaxCardCount += _Value;
                GameManager.instance.Player.MaxHand += _Value;
                UImanager.Instance.LogTextSet("�÷��̾� ���а��� ���� : " + _Value);
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
                UImanager.Instance.LogTextSet("�÷��̾� ����ī�� ���� ���� :  " + _Value);
                break;
            case CardType.Gathing:
                CardData = Resources.Load<CardDataBase>("DataTable/Card/GathingCard");
                UImanager.Instance.LogTextSet("�÷��̾� ä��ī�� ���� ���� :  " + _Value);
                break;
            case CardType.Move:
                CardData = Resources.Load<CardDataBase>("DataTable/Card/MoveCard");
                UImanager.Instance.LogTextSet("�÷��̾� �̵�ī�� ���� ���� :  " + _Value);
                break;
        }

        if(CardData !=null)
            for(int i =0; i<_Value;i++)
                GameManager.instance.Player.PR.CreateCard(CardData);

    }


}
