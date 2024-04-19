using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetUi : MonoBehaviour
{
    private  PlayerDataBase _playerData;

    
    [SerializeField]
    private Image _PlayerImage;
    [SerializeField]
    private TextMeshProUGUI _TextClassName;
    [SerializeField]
    private TextMeshProUGUI _TextHp;
    [SerializeField]
    private TextMeshProUGUI _TextDamage;
    [SerializeField]
    private TextMeshProUGUI _TextMoveCard;
    [SerializeField]
    private TextMeshProUGUI _TextAttackCard;
    [SerializeField]
    private TextMeshProUGUI _TextGathingCard;
    [SerializeField]
    private TextMeshProUGUI _TextAttackRange;
    [SerializeField]
    private TextMeshProUGUI _TextTree;
    [SerializeField]
    private TextMeshProUGUI _TextStone;
    [SerializeField]
    private TextMeshProUGUI _TextMetal;
    [SerializeField]
    private TextMeshProUGUI _TextMaxHand;


  




    public void PlayerDataSet(GameSetManager.PlayerType playerType)
    {
        //받아온 플레이어 타입에 따라 설정
        switch((int)playerType)
        {
            case 1:_playerData = Resources.Load<PlayerDataBase>("DataTable/Player/Knight");
                break;
            case 2: _playerData = Resources.Load<PlayerDataBase>("DataTable/Player/Mage");
                break;
            case 3: _playerData = Resources.Load<PlayerDataBase>("DataTable/Player/Archer");
                break;
            default:
                break;


        }
        if(_playerData != null)
        {
            _TextClassName.text = _playerData.Name;
            _TextHp.text = _playerData.MaxHp.ToString();
            _TextDamage.text = _playerData.AttackDamage.ToString();
            _TextMoveCard.text = _playerData.MoveCard.ToString();
            _TextAttackCard.text = _playerData.AttackCard.ToString();
            _TextGathingCard.text = _playerData.GathingCard.ToString();
            _TextAttackRange.text = _playerData.AttackRange.ToString();
            _TextTree.text = _playerData.Tree.ToString();
            _TextStone.text = _playerData.Stone.ToString();
            _TextMetal.text = _playerData.Metal.ToString();
            _TextMaxHand.text = _playerData.MaxHand.ToString();
            _PlayerImage.sprite = _playerData.PlayerIcon;
        }

    


    }
}
