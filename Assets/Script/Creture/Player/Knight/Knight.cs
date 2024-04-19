using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight: PlayerCreture
{

    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        //해당 캐릭터 플레이어 초기화
        if (SaveLoadManager.instance.IsLoad == false)
        { 
            _Pdata = Resources.Load<PlayerDataBase>("DataTable/Player/Knight");
            _Name = Pdata.Name;
            _AttackRange = Pdata.AttackRange;
            _AttackDamage = Pdata.AttackDamage;
            _MaxHp = Pdata.MaxHp;
            _Hp = MaxHp;
            MaxHand = Pdata.MaxHand;
            PR.Stone = Pdata.Stone;
            PR.Tree = Pdata.Tree;
            PR.Metal = Pdata.Metal;
            PR.MaxCardCount = Pdata.MaxHand;
            UnitStateUiMnager.Instance.StateGet(this);
            PR.PlayerCardInit();
        }
    }

   

}
