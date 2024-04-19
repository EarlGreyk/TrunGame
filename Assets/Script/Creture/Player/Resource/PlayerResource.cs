using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerResource : MonoBehaviour
{
    
    //플레이어가 사용하는 모든 '자원'을 관리합니다.
    private List<Card> _CardList = new List<Card>(new Card[8]);
    private List<Card> _ActiveCardList ;
    private Card _CurrentUseCard;
    
    public List<Card> CardList { get { return _CardList; } }
    public Card CurrentUsecard { get { return _CurrentUseCard; }}


    //기초자원
    private int tree;
    public int Tree { get { return tree; } set { tree = value; PRManager.instance.PrTextUpdate(); } }
    //특수자원
    private int stone;
    public int Stone { get {  return stone; } set {  stone = value; PRManager.instance.PrTextUpdate(); } }

    private int metal;
    public int Metal { get {  return metal; } set {  metal = value; PRManager.instance.PrTextUpdate(); } }
    public int CardCount { get { return _CardList.Count; } }
    private int _MaxCardCount;
    private int _CurrentCardCound = 0;
    private int _DrowCount = 3;
    private int _DrowCurrentCount = 0;
    public int MaxCardCount { get { return _MaxCardCount; } set { _MaxCardCount = value; } }
    public int DrowCount { get { return  DrowCount; } set {  DrowCount = value;  } }
    public int DrowCurrentCount { get { return _DrowCurrentCount; } set { _DrowCurrentCount = value; PRManager.instance.PrTextUpdate(); } }


    private void Start()
    {
        PRManager.instance.Prdata = this;
        PRManager.instance.PrTextUpdate();
        _ActiveCardList = new List<Card>(new Card[MaxCardCount]);
    }
    



    public void CardListSet(Card Card)
    {
        for(int i =0; i< _CardList.Count;i++)
        {
            if(_CardList[i] == null)
            {
                _CardList[i] = Card;
                return;
            }
           
        }
        _CardList.Add(Card);

    }
    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
  

    public void ActiveCardLIstSet()
    {
        
        for (int i = 0; i < _CardList.Count; i++)
        {
            if (_CardList[i] != null)
            {
                _CardList[i].CardClose();
            }
        }

        ShuffleList(_CardList);
        _CurrentCardCound = 0;
        DrowCurrentCount = _DrowCount;
        

      
        for (int i =0; i< MaxCardCount; i++)
        {
            if (_CardList[i] !=null)
            {
                if(_ActiveCardList.Count >i)
                { 
                    _ActiveCardList[i] = _CardList[i];
                }else
                {
                    _ActiveCardList.Add(_CardList[i]);
                }
                _ActiveCardList[i].CardOpen();
                _ActiveCardList[i].gameObject.transform.SetSiblingIndex(i);
                _CurrentCardCound++;
            }
        }
    }
    

    public bool CardSerch(PlayerCreture.State state)
    {
        

        for(int i =0; i < _ActiveCardList.Count; i++)
        {
            if (_ActiveCardList[i] != null)
            {
                if (state == _ActiveCardList[i].CheckCardType())
                {

                    _CurrentUseCard = _ActiveCardList[i];
                    return true;
                }

            }
            
        }

        return false;
    }
    public void CardUse()
    {
        _ActiveCardList.Remove(_CurrentUseCard);
        _CurrentUseCard.CardClose();
        _CurrentUseCard = null;
        if (_DrowCurrentCount > 0)
        {
            ActiveCardDrow();
            DrowCurrentCount--;
        }
    }
    private void ActiveCardDrow()
    {
        if (_CurrentCardCound < _CardList.Count)
        {
            _ActiveCardList.Add(_CardList[_CurrentCardCound]);
            _ActiveCardList[_ActiveCardList.Count-1].CardOpen();
            _CurrentCardCound++;
        }
    }

    public void CardRemove(int number)
    {
        _CardList.Remove(_CardList[number]);
    }

    ////////////
    ///카드 정보 관리
    ///////////
    ///
    public void PlayerCardInit()
    {


        //플레이어의 데이터를 받아와 craft를 돌려야함.
        int CardCount = 0;
        CardCount = GameManager.instance.Player.Pdata.MoveCard;
        CardDataBase CardData = null;
        CardData = Resources.Load<CardDataBase>("DataTable/Card/MoveCard");
        for (int i = 0; i < CardCount; i++)
            CreateCard(CardData);

        CardCount = GameManager.instance.Player.Pdata.AttackCard;
        CardData = Resources.Load<CardDataBase>("DataTable/Card/AttackCard");
        for (int i = 0; i < CardCount; i++)
            CreateCard(CardData);

        CardCount = GameManager.instance.Player.Pdata.GathingCard;
        CardData = Resources.Load<CardDataBase>("DataTable/Card/GathingCard");
        for (int i = 0; i < CardCount; i++)
            CreateCard(CardData);


    }


    public void CreateCard(CardDataBase carddata)
    {
        GameObject Card;

        Card = Instantiate(Resources.Load<GameObject>("UI/Card/CardBase"));
        Card card = Card.GetComponent<Card>();
        card.CardSet(carddata, UImanager.Instance.CardPop);
        CardListSet(card);
    }

}
