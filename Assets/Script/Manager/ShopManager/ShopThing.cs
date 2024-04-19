using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopThing : MonoBehaviour
{
    [SerializeField]
    private ShopDataBase _ShopData;

    public ShopDataBase ShopData { get { return _ShopData; } }

    [SerializeField]
    private TextMeshProUGUI TreePrice;
    [SerializeField]
    private TextMeshProUGUI MetalPrice;
    [SerializeField]
    private TextMeshProUGUI StonePrice;

    private void Start()
    {
        SetUi();

    }



    //�����ϳ��ϳ��� �� ��ũ��Ʈ�� [����]�ϰ�����.

    public bool PriceCheck(int Tree,int Metal,int Stone)
    {
        bool check = true;

        if (Tree < _ShopData.TreePrice)
        {
            return check = false;
        }
        else if (Metal < _ShopData.MetalPrice)
        {
            return check = false;
        }
        else if (Stone < _ShopData.StonePrice)
        {
            return check = false;
        }
        else
        {
            return check;
        }

    }
    public void PriceSet()
    {
        //�ش� �Ŵ��� ������ ������ �ʿ���.
        //����
        GameManager.instance.Player.PR.Tree -= _ShopData.TreePrice;
        GameManager.instance.Player.PR.Metal -= _ShopData.MetalPrice;
        GameManager.instance.Player.PR.Stone -= _ShopData.StonePrice;
        //���ſϷ� ����
        ShopApply();
        //���ſϷ� ������ ��������.
        _ShopData.IncreaseValue();
        SetUi();
    }
    private void SetUi()
    {
        if(_ShopData != null)
        { 
            TreePrice.text = _ShopData.TreePrice.ToString();
            MetalPrice.text = _ShopData.MetalPrice.ToString();
            StonePrice.text = _ShopData.StonePrice.ToString();
        }
    }

    //�����Ѱ� ����
    private void ShopApply()
    {
        if(_ShopData != null) 
        {
            PlayerCreture Player = GameManager.instance.Player;
            PlayerResource Pr = GameManager.instance.Player.PR;
            CardDataBase CardData = null;
            if (_ShopData.ThingType == ShopDataBase.Thing.CardMove )
            {
                CardData = Resources.Load<CardDataBase>("DataTable/Card/MoveCard");
                Debug.Log(CardData);
                Pr.CreateCard(CardData);
            }
            else if(_ShopData.ThingType == ShopDataBase.Thing.CardAttack)
            {
                CardData = Resources.Load<CardDataBase>("DataTable/Card/AttackCard");
                Pr.CreateCard(CardData);
            }
            else if(_ShopData.ThingType == ShopDataBase.Thing.CardGathing)
            {
                CardData = Resources.Load<CardDataBase>("DataTable/Card/GathingCard");
                Pr.CreateCard(CardData);
            }
            else if(_ShopData.ThingType == ShopDataBase.Thing.StatusMaxHp)
            {
                int value = (int)(Player.Pdata.MaxHp * 0.1);
                Player.MaxHp += value;
            }else if(_ShopData.ThingType == ShopDataBase.Thing.StatusDamage)
            {
                Player.AttackDamage += 1;
            }else if(_ShopData.ThingType == ShopDataBase.Thing.StatusMaxHand)
            {
                Pr.MaxCardCount += 1;
                Player.MaxHand++;
            }else if(_ShopData.ThingType == ShopDataBase.Thing.Recovery)
            {
                Player.Hp = Player.MaxHp;
                UnitStateUiMnager.Instance.StateGet(Player);
            }
            else if(_ShopData.ThingType == ShopDataBase.Thing.RemoveCard)
            {
                Pr.CardRemove(0);
            }


        }
    }


   


  
   
}
