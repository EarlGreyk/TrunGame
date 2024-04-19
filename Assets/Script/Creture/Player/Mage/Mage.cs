using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : PlayerCreture
{
    protected override void Start()
    {
        base.Start();
        //�ش� ĳ���� �÷��̾� �ʱ�ȭ
        if (SaveLoadManager.instance.IsLoad == false)
        {
            _Pdata = Resources.Load<PlayerDataBase>("DataTable/Player/Mage");
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

    protected override void Update()
    {
        base.Update();
    }
 


}
