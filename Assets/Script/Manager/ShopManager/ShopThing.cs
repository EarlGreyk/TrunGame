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



    //물건하나하나가 이 스크립트를 [소유]하고있음.

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
        //해당 매니저 접근은 조정이 필요함.
        //구매
        GameManager.instance.Player.PR.Tree -= _ShopData.TreePrice;
        GameManager.instance.Player.PR.Metal -= _ShopData.MetalPrice;
        GameManager.instance.Player.PR.Stone -= _ShopData.StonePrice;
        //구매완료 적용
        ShopApply();
        //구매완료 데이터 가격증가.
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

    //구매한거 설정
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
