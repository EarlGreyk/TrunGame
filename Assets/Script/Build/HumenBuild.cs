using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumenBuild : MonsterBuild
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        BuildDataBase buildData = Resources.Load<BuildDataBase>("DataTable/MonsterBuild/HumenBuild");
        if (!SaveLoadManager.instance.IsLoad)
        {
            _isturn = false;
            _MaxHp = buildData.MaxHp;
            _Hp = _MaxHp;
            _CurrentMonster = 0;
            _MaxMonster = buildData.MaxMonster;
            _CreateValue = buildData.CreateValue;
            _Value = 0;
            _Range = buildData.Range;
        }
        
        
    }

    
}
