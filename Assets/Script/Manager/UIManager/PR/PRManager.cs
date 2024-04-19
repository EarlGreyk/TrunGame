using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PRManager : MonoBehaviour
{
    public static PRManager instance;
    private PlayerResource _Prdata;
    public PlayerResource Prdata { set { _Prdata = value; } }

    [SerializeField]
    private TextMeshProUGUI Pr_Ston;
    [SerializeField]
    private TextMeshProUGUI Pr_Metal;
    [SerializeField]
    private TextMeshProUGUI Pr_Tree;
    [SerializeField]
    private TextMeshProUGUI Pr_Card;
    [SerializeField]
    private TextMeshProUGUI PR_DrowCount;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
  



    public void PrTextUpdate()
    {
        if(gameObject.activeSelf && _Prdata !=null)
        {
            Pr_Tree.text = _Prdata.Tree.ToString();
            Pr_Ston.text = _Prdata.Stone.ToString();
            Pr_Metal.text = _Prdata.Metal.ToString();
            Pr_Card.text = _Prdata.CardCount.ToString();
            PR_DrowCount.text = _Prdata.DrowCurrentCount.ToString();

        }
    }
   
    //턴종료버튼에서 작동하고 있는 함수 추후 처리해야함
    public void PlayerTurnEnd()
    {
        GameManager.instance.Player.TurnSet(false);
    }

  

}
