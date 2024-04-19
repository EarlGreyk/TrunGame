using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footman : MonsterCreture
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        MonsterDataBase monsterDataBase = Resources.Load<MonsterDataBase>("DataTable/Monster/Nomal/Footman");
        if(monsterDataBase != null )
        {
            _Name = monsterDataBase.Name;
            MaxHp = monsterDataBase.MaxHp;
            Hp = MaxHp;
            _AttackRange = monsterDataBase.AttackRange;
            _TargetRange = monsterDataBase.TargetRange;
            _MaxAcionCount = monsterDataBase.MaxActionCount;
            _AttackDamage = monsterDataBase.AttackDamage;
            _AcionCount = _MaxAcionCount;
            _HpSlider.SetValue(Hp, MaxHp);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

 
}
