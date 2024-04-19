using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private GameObject ParentObject;
    private CardDataBase _CardData;
    public CardDataBase CardData { get { return _CardData; } }

    
    [SerializeField]
    private Image _IconSprite;
    [SerializeField]
    private TextMeshProUGUI _CardName;
    [SerializeField]
    private TextMeshProUGUI _CardDescription;


    

   

 
    private void DataInit()
    {
        if (ParentObject != null)
            gameObject.transform.SetParent(ParentObject.transform);
        
        if(_CardData !=null)
        {
            _IconSprite.sprite = Resources.Load<Sprite>(_CardData.SpritePath);
            _CardName.text = _CardData.Name;
            _CardDescription.text = _CardData.Desc;
        }else
        {
            _CardName.text = _CardData.Name;
            _CardDescription.text = _CardData.Desc;
        }
        CardClose();

    }
    public void CardSet(CardDataBase Carddata , GameObject parent)
    {
        _CardData = Carddata;
        ParentObject = parent;
        DataInit();
    }


    public void CardOpen()
    {
        
        gameObject.SetActive(true);
    }
    public void CardClose()
    {
        gameObject.SetActive(false);
    }

    public PlayerCreture.State CheckCardType()
    {
        PlayerCreture.State state = PlayerCreture.State.None;

        switch (_CardData.DataType)
        {
            case CardDataBase.ValueType.Move:
                state = PlayerCreture.State.Move;
                break;
            case CardDataBase.ValueType.Attack:
                state = PlayerCreture.State.Attack;
                break;
            case CardDataBase.ValueType.Gathing:
                state = PlayerCreture.State.gathering;
                break;
        }

        return state;
    }




    

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject card = UImanager.Instance.CardFocus;
        card.SetActive(false);
    }



   

}
